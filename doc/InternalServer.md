# 20231012
原打包机ip地址更换为域名：
10.60.70.4 -> tob.spatio-inc.com
打包机从工位搬入机房（小会议隐藏门内）

相关常用地址变更如下：
FTP (HTTP protocol):  http://tob.spatio-inc.com:8099/ftp/
H5 plant latest building:  http://tob.spatio-inc.com:8099/ftp/build_output/cocos/plant/latest/?mode=dev&user-id=001
Jenkins: http://tob.spatio-inc.com:8088/
Redmine: http://tob.spatio-inc.com:8077/redmine/
Gitblit: http://tob.spatio-inc.com:10101/repositories/



# Computer Infomation
IP: 10.60.80.2
Location: B-12-1
CPU: i7-9700
MEM: 16GB
DISK: C(SSD 256G) D(HD 1T)
It is property of Xing Chong.

Administrator
Abc$1234

# FTP
FileZilla Server

* Location root:
```shell
D:\ftp
```
|   Sub path   |     Purpose     |
| ------------ | --------------- |
| build_output | Jenkins outputs |
| doc          | Documents       |
| tools        | Tools           |

* Visit method 1:
ftp://10.60.80.2:21
username: download
password: 

* Visit method 2:
http://10.60.80.2:8099/ftp/

# Http 
http://10.60.80.2:8099

| Sub path (Remote) |                                 Sub path (Location)                                 |           Purpose           |
| ----------------- | ----------------------------------------------------------------------------------- | --------------------------- |
| /jenkins/git/     | C:\Users\Administrator\AppData\Local\Jenkins\.jenkins\workspace\build_demo1_andorid | git repository              |
| /ftp/             | D:\ftp                                                                              | download ftp files via http |

# Redmine
http://10.60.80.2:8077/redmine/
admin
Abc$1234

* database account
    > root
    > 11111111

* Local path
```shell
D:\Bitnami\redmine-5.0.0-1
```

# Jenkins
http://localhost:8088/
admin
Abc$1234

# Gitblit
http://10.60.80.2:10101/repositories/
D:\soft\gitblit-1.9.3

admin
Abc$1234

# SVN
VisualSVN
https://10.60.80.2:456
http://10.60.80.2:8099/ftp/tools/TortoiseSVN-1.14.3.29387-x64-svn-1.14.2.msi

# JDK & JRE
D:\Program Files\Java
| Version    | Purpose    |
| --- | --- |
| jdk1.8.0_45    |     |
| jdk1.8.0_45    |  For Tomcat 9   |
| jdk1.8.0_45    |     |

# Untiy Accelerator
[metrics-page](http://10.60.80.2/dashboard/#/metrics-page)
10.60.80.2:10080
![](vx_images/590535510247603.png)
admin
Abc$1234