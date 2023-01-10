IF EXIST "C:\Windows\System32\drivers\etc\hosts.bak" (
	copy C:\Windows\System32\drivers\etc\hosts.bak C:\Windows\System32\drivers\etc\hosts
) ELSE (
	copy C:\Windows\System32\drivers\etc\hosts C:\Windows\System32\drivers\etc\hosts.bak
)

echo 10.60.80.6    server.tob.yidian-inc.com>> C:\Windows\System32\drivers\etc\hosts
echo 10.60.80.6    gitblit.tob.yidian-inc.com>> C:\Windows\System32\drivers\etc\hosts
echo 10.60.80.6    svn.tob.yidian-inc.com>> C:\Windows\System32\drivers\etc\hosts
echo 10.60.80.6    redmine.tob.yidian-inc.com>> C:\Windows\System32\drivers\etc\hosts
echo 10.60.80.6    jenkins.tob.yidian-inc.com>> C:\Windows\System32\drivers\etc\hosts
echo 10.60.80.6    unityaccelerator.tob.yidian-inc.com>> C:\Windows\System32\drivers\etc\hosts
echo 10.60.80.6    http.tob.yidian-inc.com>> C:\Windows\System32\drivers\etc\hosts
echo 10.60.80.6    ftp.tob.yidian-inc.com>> C:\Windows\System32\drivers\etc\hosts
pause