#!/usr/bin/python
import argparse
import os
import subprocess
import sys
import tempfile
import threading

import symphony


def parse():
    p = argparse.ArgumentParser(usage='copy_mp3s.py scorename length')
    p.add_argument('--test', action='store_true', help='Internal use.')
    p.add_argument(
        'songname', help='Name of the song inside the source_Music folder.')
    p.add_argument('length', help='Length of the result in mp3splt (<m>.<s>).')
    return p.parse_args()


def check_sources(srcs):
    purenames = set(symphony.splitp(f)[1] for f in srcs)
    extra = purenames.difference(symphony.INSTRUMENTS)
    if extra:
        sys.exit('Found extra mp3 files in source: %s' % ', '.join(extra))
    missing = set(symphony.INSTRUMENTS).difference(purenames)
    if missing:
        sys.exit('Missing mp3 files: %s' % ', '.join(missing))


def get_score_and_tgtdir(srcdir):
    scorename = os.path.split(srcdir)[-1]
    tgtdir = os.path.join(symphony.USCORES, scorename)
    if not os.path.exists(tgtdir):
        os.makedirs(tgtdir)
    return scorename, tgtdir


_stdout_happening = False
_stdout_lock = threading.Lock()


def _trap_stdout(cmdline):
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

tempd = tempfile.mkdtemp()


def compress(src, tgt, length):
    basename = os.path.basename(src)
    purename, ext = os.path.splitext(basename)
    split_cmdline = ['mp3splt', '-d', tempd, '-o', purename, src, '0.0', length]
    _trap_stdout(split_cmdline)
    abstemp = os.path.join(tempd, purename + ext)
    assert os.path.isfile(abstemp)

    lame_opts = ['-m', 'm', '-V', '9', '-h']
    lame_cmdline = ['lame'] + lame_opts + [abstemp, tgt]
    if not os.path.exists(os.path.dirname(tgt)):
        os.makedirs(os.path.dirname(tgt))
    _trap_stdout(lame_cmdline)
    os.remove(abstemp)


def get_srcdir(songname):
    srcdir = os.path.join(symphony.SMUSIC, songname)
    if not os.path.isdir(srcdir):
        sys.exit('%s is not a directory. Check your song name.' % srcdir)
    return srcdir


def main():
    opts = parse()
    srcdir = get_srcdir(opts.songname)

    scorename, tgtdir = get_score_and_tgtdir(srcdir)
    srcs = list(symphony.listfiles(srcdir, '*.mp3'))
    check_sources(srcs)

    def maketgt(src):
        return os.path.join(
            tgtdir, '%s_%s' % (scorename, os.path.basename(src)))
    src_and_tgts = [(s, maketgt(s)) for s in srcs]
    if opts.test:
        del src_and_tgts[1:]
    symphony.say('Compressing/copying %s music files to %s.',
                 len(src_and_tgts), tgtdir)
    symphony.pmap(lambda a: compress(a[0], a[1], opts.length), src_and_tgts)
    symphony.say('Finished copying %s files.' % len(src_and_tgts))


if __name__ == '__main__':
    main()
