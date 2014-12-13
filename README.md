# EpicTTS

EpicTTS is an open source software for all things text-to-speech. This software was developed because of glaring omissions from many free TTS programs. Some omissions include:

1. Lack of support for opening a text file via command line options (and thus, the impossibility of opening text files with the TTS program)
2. Lack of support for saving to a wave file.
3. Arbitrary, undocumented limitations on the length of the saved wave file.

EpicTTS solves all these problems.

The current version of EpicTTS is 0.9.1. EpicTTS uses [semantic versioning](http://semver.org/).

## Changelog

### Version 0.9.1

1. Fix a bug where right-clicking on the "..." button next to the file to export, did not cause the Explorer context menu to appear.
2. Create an MSI installer instead of an EXE installer.
