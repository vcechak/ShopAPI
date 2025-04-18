Prerequisities: DOCKER Desktop

"docker-compose up -d" -> Docker composition "docker-compose down" -> Docker decomposion (for restart) "docker ps" -> information regarding containers in docker

**1st Start:**

run kafka: "docker-compose up -d" in terminal, root Shop folder (where is located docker-compose.yml)

get kafka container id "docker ps" // also should be visible in docker desktop

Create TOPIC PAYMENTS IN KAFKA "docker exec -it kafka-topics --create \ --topic payments --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1"

**Afterwards** you should be able to start wanted containers directly in Docker Desktop
