# Resovle Error of U3D WebGL
https://blog.csdn.net/Highning0007/article/details/124109682  
And another Warning:
```
HTTP Response Header "Content-Type" configured incorrectly on the server for file Build/webgl.wasm , should be "application/wasm". Startup time performance will suffer.
```

# How to build JAR and put to tomcat
https://www.jianshu.com/p/5cc7d87c19f0

* web.xml
    ```xml
        <filter>
            <filter-name>httpResponseHeaderFilter</filter-name>
            <filter-class>com.vking.power.web.filter.HttpResponseHeaderFilter</filter-class>
        </filter>
    
        <filter-mapping>
            <filter-name>httpResponseHeaderFilter</filter-name>
            <url-pattern>*.wasm</url-pattern>
        </filter-mapping>
    ```
 * server.xml
    ```xml
    <Context path="" docBase="D:/output/" debug="0" reloadable="true" crossContext="true" />
    ```
 
# How to build JAR in Idea
https://blog.csdn.net/hotdust/article/details/56277138?utm_source=copy

# How to resovle compile error: javax.servlet不存在
https://blog.csdn.net/QQ826688096/article/details/89414128

# Resovle error for java runtime version
https://blog.csdn.net/weixin_42505246/article/details/114164793
```
java.lang.UnsupportedClassVersionError: com/vking/power/web/filter/HttpResponseHeaderFilter has been compiled by a more recent version of the Java Runtime (class file version 62.0), this version of the Java Runtime only recognizes class file versions up to 52.0
```
Find Java EE version of Tomcat: https://tomcat.apache.org/index.html

