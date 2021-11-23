# MoonWalker - Makefile
# (C) 2008, University of Twente - Formal Methods and Tools group
# This makefile has only been tested on Mac OS X 10.4.11.

LIB=../lib
BIN=../../bin
SRC=*.cs

mono: moonwalker.exe

moonwalker.exe: $(SRC) makefile
	gmcs -m:MMC.MonoModelChecker -out:moonwalker.exe -optimize+  \
		-r:$(LIB)/C5.dll,$(LIB)/Mono.Cecil.dll $(SRC)

cildump : cildump.exe
cildump.exe : CILDumper.cs
	gmcs -m:MMC.CILDumper -out:cildump.exe -r:$(LIB)/Mono.Cecil.dll CILDumper.cs

install: moonwalker.exe
	if [ ! -d $(BIN) ]; then mkdir $(BIN); fi
	cp -f -p $(LIB)/* $(BIN)
	cp -f -p moonwalker.exe $(BIN)

dist: install
	if [ -d bin ]; then rm -r -f bin; fi
	cp -p -R $(BIN) .
	zip -o -rp moonwalker.zip bin examples LICENSE.txt README.html
	rm -r -f bin

clean:
	rm -f moonwalker.exe cildump.exe
	rm -f moonwalker.zip

# The following target has not been tested, lately.
# microsoft:
#	csc /out:moonwalker.exe /o+ /m:MMC.MonoModelChecker /r:C5.dll,Mono.Cecil.dll  *.cs