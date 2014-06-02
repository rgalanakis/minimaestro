from __future__ import print_function
import contextlib
import fnmatch
from multiprocessing.pool import ThreadPool
import os
from os.path import join

DEVNULL = open(os.devnull, 'wb')

PROJROOT = os.path.abspath(join(os.path.dirname(__file__), '..'))

UASSETS = join(PROJROOT, 'unity', 'Assets')
UMUSIC = join(UASSETS, 'Music')
USCORES = join(UMUSIC, 'scores')
UART = join(UASSETS, 'Art')
UUI = join(UART, 'UI')

SMUSIC = join(PROJROOT, 'source_Music')
SUI = join(PROJROOT, 'source_UI')

INSTRUMENTS = (
    'trumpet',
    'tuba',
    'horn',

    'flute',
    'clarinet',

    'drum',
    'cymbal',
    'xylophone',
    'piano',

    'violin',
    'harp'
)


def listfiles(path, pattern='*.*'):
    for f in os.listdir(path):
        if fnmatch.fnmatch(f, pattern):
            yield join(path, f)


def map2(func, seq):
    for a, b in seq:
        func(a, b)


_pool = ThreadPool()


def pmap(func, seq):
    _pool.map(func, seq)


def say(s, *a):
    print(s % a)


def splitp(path):
    head, tail = os.path.split(path)
    pure, ext = os.path.splitext(tail)
    return head, pure, ext


@contextlib.contextmanager
def chdir(d):
    old = os.getcwd()
    os.chdir(d)
    try:
        yield
    finally:
        os.chdir(old)
