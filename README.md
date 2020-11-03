
# Game manager
A simple manager for controlling and lending your games

#### How to run
The solution has 3  containers: database, api and web

```sh
docker-compose build
docker-compose up
```
They will be available on the following ports:
>MySQL: 3306

>API: 5001

>Web: 5002

## 


### API
API runs https on port 5001.

Swagger doc is located at https://localhost:5001/swagger/index.html 
\
You can try out the API using swagger. Call **/api/user/authenticate** to get a token, and copy it on the authorize part.
\
Use the following credentials:

>**username:** admin

>**password:** W;L$n,bmXt4!r]s

### Web
You can access the web ui at: https://localhost:5002/
\
The web ui runs on port 5002. Unfortunately, the web interface is still incomplete. Currently, you cannot add/edit/remove friends, but you can do this with games, including lending and returning.
##
### MySQL
If needed, the database **gamemanager** can be accessed on port 3306, using the following credentials:
>**username:** api  

>**password:** K>k={&}f3bp,LR.