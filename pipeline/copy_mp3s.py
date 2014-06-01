#!/usr/bin/python
import os
import subprocess
import sys
import threading

import symphony


def check_sources(srcs):
    if len(srcs) != symphony.INSTRCNT:
        sys.exit('Expected %s music files, found %s.' % (
            len(srcs), symphony.INSTRCNT))
    purenames = set(symphony.splitp(f)[1] for f in srcs)
    diff = purenames.difference(symphony.INSTRUMENTS)
    if diff:
        sys.exit('Instruments differ: %s' % ', '.join(diff))


def get_score_and_tgtdir(srcdir):
    scorename = os.path.split(srcdir)[-1]
    tgtdir = os.path.join(symphony.UMUSIC, scorename)
    if not os.path.exists(tgtdir):
        os.makedirs(tgtdir)
    return scorename, tgtdir


_stdout_happening = False
_stdout_lock = threading.Lock()


def compress(src, tgt):
    opts = ['-m', 'm', '-V', '9', '-h']
    cmdline = ['lame'] + opts + [src, tgt]
    global _stdout_happening
    with _stdout_lock:
        i_am_stdout = False
        stdout = symphony.DEVNULL
        if not _stdout_happening:
            _stdout_happening = True
            i_am_stdout = True
            stdout = None
    subprocess.check_call(cmdline, stderr=stdout)
    if i_am_stdout:
        _stdout_happening = False


def get_srcdir():
    if len(sys.argv) == 1:
        sys.exit('usage: copy_mp3s.py scorename')

    srcdir = os.path.join(symphony.SMUSIC, sys.argv[1])
    if not os.path.isdir(srcdir):
        sys.exit('%s is not a directory.' % srcdir)

    return srcdir


def main():
    srcdir = get_srcdir()

    scorename, tgtdir = get_score_and_tgtdir(srcdir)
    srcs = list(symphony.listfiles(srcdir, '*.mp3'))
    check_sources(srcs)

    def maketgt(src):
        return os.path.join(
            tgtdir, '%s_%s' % (scorename, os.path.basename(src)))
    src_and_tgts = [(s, maketgt(s)) for s in srcs]
    symphony.say('Compressing/copying %s music files to %s.',
                 len(src_and_tgts), tgtdir)
    symphony.pmap(lambda a: compress(a[0], a[1]), src_and_tgts[:1])
    # Needed to bring up prompt, lame's curses will hide it.
    sys.exit(0)


if __name__ == '__main__':
    main()
