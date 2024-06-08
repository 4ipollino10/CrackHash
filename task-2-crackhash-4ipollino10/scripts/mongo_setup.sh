#!/bin/bash
sleep 10

mongosh --host mongo_primary:27017 <<EOF
  var cfg = {
    "_id": "myReplicaSet",
    "version": 1,
    "members": [
      {
        "_id": 0,
        "host": "mongo_primary:27017",
        "priority": 2
      },
      {
        "_id": 1,
        "host": "mongo_secondary1:27017",
        "priority": 1
      },
      {
        "_id": 2,
        "host": "mongo_secondary2:27017",
        "priority": 0
      }
    ]
  };
  rs.initiate(cfg);
EOF