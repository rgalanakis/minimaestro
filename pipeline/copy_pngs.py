#!/usr/bin/python
import os

import symphony


def strip_digit_part(path):
    head, tail = os.path.split(path)
    parts = tail.split('_')
    no_nums = [p for p in parts if not p.isdigit()]
    if parts == no_nums:
        return None
    return '_'.join(no_nums)


def get_src_and_tgts():
    for path in symphony.listfiles(symphony.SUI, '*.png'):
        tgtname = strip_digit_part(path)
        if tgtname is None:
            continue
        yield path, os.path.join(symphony.UUI, tgtname)


def main():
    path_pairs = list(get_src_and_tgts())
    symphony.say('Moving %s pngs.' % len(path_pairs))
    symphony.map2(os.rename, path_pairs)


if __name__ == '__main__':
    main()
