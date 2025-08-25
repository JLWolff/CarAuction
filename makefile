.PHONY: build db-up migrate run all

build:
	docker-compose build

run-server: 
	docker-compose up -d server

run-all: run-server
	docker-compose up -d client 
