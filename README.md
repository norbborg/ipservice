# Ip Service
Ip service that gets ip details from IP Stack.

## Features
- Get ip details
- Batch update ip details
- Get update job status

## How to run
To run this project using docker one need to have docker up and running on the machine. 

```
cd {repo-directory}/docker
```
```
docker-compose up --build
```

## How to use
- Visit <http://localhost:5002/docs> in a web browser to access the swagger UI.
- Visit <http://localhost:5002/hangfire> in a web browser to access the hangfire dashboard where one can see more information about the background jobs.
