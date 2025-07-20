# 💼 Technical Challenge – Transaction Validation System

This project is part of a technical challenge focused on building a robust microservices-based architecture to handle and validate financial transactions. The system includes asynchronous communication between services and real-time antifraud validation using Apache Kafka.

---

## ⚙️ Architecture Overview

The solution follows a microservices-based architecture with decoupled services that communicate via message passing. Each service adheres to **Hexagonal Architecture (Ports and Adapters)** to keep the business logic clean, modular, and testable.

### 🔁 Services

#### 🧾 TransactionService

Responsible for:

- Receiving financial transactions through a RESTful API
- Persisting the transaction data in PostgreSQL
- Publishing a message to Kafka to notify the antifraud validation service

#### 🔍 AntiFraudService

Responsible for:

- Listening to submitted transactions from Kafka
- Running validation rules to detect fraudulent activity
- Sending back the evaluation result to update the transaction status

---

## 🧰 Technologies Used

- **.NET 8**
- **Apache Kafka**
- **PostgreSQL**
- **Docker & Docker Compose**
- **Entity Framework Core**
- **Kafka Confluent Client**
- Clean architecture principles (Application, Domain, Infrastructure)

---

## 🔌 Inter-service Communication

Kafka acts as the backbone for asynchronous communication:

- TransactionService produces messages to the `transaction-submitted` topic.
- AntiFraudService consumes those messages, processes them, and produces validation results.

---

## 🚀 How to Run the Project Locally

### Prerequisites

- Docker & Docker Compose installed on your system

### Step 1: Clone the Repository

```bash
git clone https://github.com/josepalacios/bankchallenge.git
cd RetoTecnico
```

### Step 2: Start All Services

```bash
docker-compose up --build
```

This will start:

- PostgreSQL (port 5432)
- Kafka & Zookeeper
- TransactionService (port 5001)
- AntiFraudService (background worker)

---

## 📡 API Endpoints

### ✅ Create a New Transaction

```http
POST http://localhost:5001/api/transaction
Content-Type: application/json
```

**Request body:**

```json
{
  "sourceAccountId": "a1111111-1111-1111-1111-111111111111",
  "targetAccountId": "b2222222-2222-2222-2222-222222222222",
  "transferTypeId": 1,
  "value": 1800
}
```

**Response:**

```json
{
  "transactionExternalId": "dfeb73e7-ed22-4d15-b9e1-c73518e1ba25",
  "createdAt": "2025-07-20T18:46:20.257Z"
}
```

---

### 📅 Get Transactions by Account and Date

```http
GET http://localhost:5001/api/transaction/by-account-and-date?sourceAccountId=a1111111-1111-1111-1111-111111111111&date=2025-07-20
```

Returns all transactions from a specific account on a specific date.

**Example response:**

```json
[
  {
    "transactionExternalId": "c89eeac4-1104-43e1-96fd-fdd9b27facc4",
    "sourceAccountId": "a1111111-1111-1111-1111-111111111111",
    "targetAccountId": "b2222222-2222-2222-2222-222222222222",
    "transferTypeId": 1,
    "value": 1800,
    "status": 1,
    "createdAt": "2025-07-20T15:43:12.387Z"
  }
]
```

---

## 👤 Author

**José Palacios**  
GitHub: [https://github.com/josepalacios](https://github.com/josepalacios)
