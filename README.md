# SharpPhisher
 A C# Phishing tool aimed to assist Red Teams in security assessments by phishing credentials from unaware users.

The tool utilizes [CredUIPromptForWindowsCredentials WinAPI function](https://docs.microsoft.com/en-us/windows/win32/api/wincred/nf-wincred-creduipromptforwindowscredentialsa) and Managed DLL Exports to capture credentials when executed using rundll32.exe on target system, the captured credentials are transmitted over HTTPS to a remote Flask Python server.

The Managed DLL exports are implemented for an attacker to mimic legitimate activity like `rundll32.exe shell32.dll,Control_RunDLL` in order to avoid raising suspicion/detection by Blue Teams and users as much as possible, for example: `rundll32.exe shell64.dll,Control_RunDLL`

The captured credentials would be written to a CSV file (user_creds.csv) on disk which will contain the following information:
* ipv4 & hostname of the target system
* username & password of the target user.

Feel free to copy and modify this software

This project wouldn't be possible without: https://github.com/3F/DllExport <br>
Thanks to [mayuki](https://gist.github.com/mayuki/339952/c7d9489f7bf06868f14923bd02d590fb8203557a) for the Windows Credential UI Helper, it saved me a lot of work...

## Configuration
- Adjust the IP Address to match your own address hosting the Flask server by changing the `$flask_ip` in the `/payloads_credui/SharpPhisher_CredUI_dll` project. 
- Regenerate the certificate used by the Flask server in `/python_server/server.py`

**Note:**<br>
SharpPhisher_CredUI_console does not require any configuration changes since it is writing the output to console

## Execution
You can execute SharpPhisher Managed DLL via rundll32 using one of the DLL exports:<br>
```
rundll32.exe SharpPhisher.dll Control_RunDLL
rundll32.exe SharpPhisher.dll Control_RunDLL appwiz.cpl,,0
```
**Notes:**<br> 
- Use one of the following binaries after build `/bin/Release/x64/` or `/bin/Release/x86/` others will not work.<br>
- Each export has a different UI.

## Similiar software:
https://github.com/matterpreter/OffensiveCSharp/tree/master/CredPhisher

## Usefull information
https://github.com/3F/DllExport

https://github.com/p3nt4/RunDLL.Net

https://3xpl01tc0d3r.blogspot.com/2019/11/managed-dll-exports-and-run-via-rundll32.html

https://unmanagedexports.wordpress.com/2016/09/29/unmanaged-exports/?msclkid=26402a97b5c011ecafd1561cb48595cb


#  DISCLAIMER:
This software is intended for use in authorized security assesments! Any misuse of the software or damaged caused by it is your own responsibility.
