
// Prerequisities, DOCKER Desktop

"docker-compose up -d" -> Docker composition
"docker-compose down" -> Docker decomposion (for restart) 
"docker ps" -> information regarding containers in docker


SETUP:
1. run kafka: "docker-compose up -d"  in terminal, root Shop folder (where is located docker-compose.yml)

2. get kafka container id "docker ps" // also should be visible in docker desktop

3. Create TOPIC PAYMENTS IN KAFKA
	"docker exec -it <kafka-container-id> kafka-topics --create \ --topic payments --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1"

