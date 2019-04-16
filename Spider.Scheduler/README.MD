1) Build:
docker build . -t radar/scheduler.api -f Dockerfile

2) Run:
docker container run -p 8000:80 radar/scheduler.api SchedulerAPI

docker container run -p 8000:80 -e CONNECTION_STRING=mongodb://localhost:27017 radar/scheduler.api SchedulerAPI


3) Remove:
docker rmi -f $(docker images -q)