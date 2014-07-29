import os

import symphony


def tp(path):
    return path[len(os.getcwd()):]


def report(msg, *args):
    print msg % args


def report_mp3(path):
    kb = os.path.getsize(path) / 1024
    # report('  %s: %sKB', tp(path), mb)
    return kb


def report_folder(folder):
    totalkb = 0
    # report('Files in %s:', tp(folder))
    for mp3 in symphony.iter_files(folder, '*.mp3'):
        totalkb += report_mp3(mp3)
    mb = totalkb / 1024.0
    report('  Total for %s: %0.2fMB', tp(folder), mb)
    return mb


def report_root(folder):
    totalmb = 0
    for d in [d for d in symphony.listfiles(folder, '*') if os.path.isdir(d)]:
        totalmb += report_folder(d)
    report('Total for %s: %0.2fMB', tp(folder), totalmb)


def report_all():
    report_root(symphony.SMUSIC)
    report_root(symphony.USCORES)


if __name__ == '__main__':
    report_all()
