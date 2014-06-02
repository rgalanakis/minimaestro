A single song in Mini Maestro is composed of 11 individual tracks,
one for each instrument (see [Instruments][] for a list of instruments).

# Before Starting

You need to get set up with some things before starting work.

## Getting added to the project

Sign up for an account on <https://www.github.com>,
and email Rob (<rob.galanakis@gmail.com>) to get added to the GitHub
project.

## Installing software

You must install the LAME mp3 encoder,
Git for source control,
and Python, which are all open source.
I've broken it down by operating system.

### Linux

Run the following from a terminal. You may need to substitute
your system's package manager.

    $ sudo apt-get install lame
    
You already have Python and Git.

### OSX

I suggest installing `LAME` through `homebrew`.
You can also download a `.dmg` from <http://lame.buanzo.org/#lameosxdl>
and install it that way if you want.

First install homebrew if you don't already have it.
Open Terminal, and run the following.
Follow the instructions when prompted:

    $ ruby -e "$(curl -fsSL https://raw.github.com/Homebrew/homebrew/go/install)"
    
Then from Terminal run:

    $ brew install lame
    
Finally, to install Git, you can just run `git` from Terminal:

    $ git
    
You will be prompted to install it through XCode developer tools.
    
You're done! You already have Python.

### Windows

Download <http://lame.buanzo.org/Lame_v3.99.3_for_Windows.exe>
and install it.

Next, download Python and install it from
<https://www.python.org/download/windows>
if you don't have it already.
Versions 2.6, 2.7, 3.3, or 3.4 are fine.

Make sure `python` and `lame` are in your `PATH`. 
Talk to Rob or Mumm if you need help.

Finally, download and install Git.
If you're not familiar with Git, just use the GitHub application.
Download and install it from <https://windows.github.com/>.

# Cloning the Project

Navigate to the directory you want to create the project in,
and run from a terminal:

    $ git clone https://github.com/rgalanakis/minimaestro.git
    
This will sync down the project.

# Creating a Score

You should create a new folder in the `source_Music` folder for your song.
All source files (`.midi`, `.pdf`, `.band`, whatever) go here.

Then you create your music.
The composition should have a track for each instrument.
See [Instruments][] for a list of instruments
available in Mini Maestro.

# Exporting a Score

When you are ready to export, you must create eleven `.mp3` files, 
one for each instrument. They must be named the same as the instrument
and should be all-lowercase.
Creating these `.mp3` files is up to you.

Run the `pipeline/copy_mp3s.py` file and pass it the song you are exporting.
For example:

    $ python pipeline/copy_mp3s.py mysong
    
This will copy and compress the songs into the correct directory.
If there's a problem, the script should tell you what it is.
If you're having trouble, email Rob.

# Committing your Work

If you are familiar with Git, you can just add and submit your work.
If not, you can run the following code to commit all your changes
to the GitHub repository:

    $ python pipeline/commit_music.py

# Instruments

The instruments are:

- Brass:
    - Trumpet
    - Tuba
    - French Horn
- Woodwinds:
    - Flute
    - Clarinet
- Percussion:
    - Drum
    - Cymbal
    - Xylophone/Bell
    - Piano
- Strings:
    - Violin
    - Harp