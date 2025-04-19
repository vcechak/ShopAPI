# Docker Instructions

## Prerequisites
- **DOCKER Desktop** installed.

## Common Docker Commands
- `docker-compose up -d` → Start Docker composition.
- `docker-compose down` → Stop Docker composition (useful for restarts).
- `docker ps` → View information about running containers in Docker.

---

## **1st Start**

1. **Run Kafka**  
   Execute the following command in the terminal from the root Shop folder (where `docker-compose.yml` is located):
   
   "docker-compose up -d"

3. **Get Kafka Container ID**  
   Use the following command to retrieve the Kafka container ID:  

   "docker ps"

   Alternatively, you can view the container in Docker Desktop.

3. **Create Kafka Topic: PAYMENTS**  
   Run the following command to create the `payments` topic in Kafka:

   "docker exec -it <kafka-container-id> kafka-topics --create \ --topic payments --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1"

## **Afterwards**
  You can start the desired containers directly in Docker Desktop.
