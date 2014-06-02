import subprocess
import sys


import symphony


def add():
    with symphony.chdir(symphony.SMUSIC):
        subprocess.check_call(['git', 'add', '.'])
    with symphony.chdir(symphony.USCORES):
        subprocess.check_call(['git', 'add', '.'])


def commit_and_push():
    args = ['git', 'commit', '-m', 'auto-commit from %s' % sys.argv[0]]
    subprocess.check_call(args)
    subprocess.check_call(['git', 'push'])


if __name__ == '__main__':
    add()
    commit_and_push()
