The difference between ShellLinkA and ShellLinkW are how the strings are encoded.

The Shell Link classes have been heavily reorganized to clean up some mess.
Before the reorganization, the ShellLink class had a lot of code that looked like this:

if(child is ShellLinkA A){
	A.Method1();
} else if(child is ShellLinkB B){
	B.Method1();
}

A bit of work has been done to create a common class/interface that forwards to the appropriate members of either A or B.