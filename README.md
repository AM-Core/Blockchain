# 🚀 AM-Core Blockchain

**Developed by: Amir & Moein Hosseini** 🤝🔥

Welcome to our joint project! We built this Blockchain simulation from scratch using **C#** and **.NET 8**. It's not just a simple script; we designed it with a clean architecture (Domain, Application, ConsoleApp) to make it robust and cool! 😎🏗️

## 🧠 What is this project?

This is a fully functional **Blockchain CLI (Command Line Interface)**. We created a system that simulates how real cryptocurrencies work. It manages a "Mempool" (a waiting room for transactions ⏳), handles mining operations using Proof of Work, and securely links blocks together using cryptographic hashes. 🔗🔒

We built this to understand the core concepts of distributed ledgers, difficulty adjustment, and transaction validation.

## 🛠️ Key Features & Options

Here is what you can do in our console application:

- **`SetDifficulty(number)`** 🧱  
  This controls the "Proof of Work" difficulty.
  *   *How it works:* We adjust how hard the math problem is for the miner. Higher number = harder to mine! 😓

- **`AddTransactionToMempool(filepath)`** 💸  
  Loads transaction data from a JSON file into our system.
  *   *How it works:* Reads the file and queues the transactions in the Mempool, waiting to be picked up by a miner. 📥

- **`EvictMempool(count)`** 🧹  
  Manages the memory pool size.
  *   *How it works:* If the waiting room gets too crowded or old, we can kick out a specific number of transactions. 👋

- **`MineBlock`** ⛏️💎  
  The main event!
  *   *How it works:* Takes transactions from the Mempool, calculates the hash that matches the current difficulty, and adds a shiny new block to the chain! ✨

- **`Help`** 🆘  
  Shows the list of commands if you forget them.

## 🏃 How to Run
1. Open the project in your favorite C# IDE (like Visual Studio or Rider).
2. Run the `ConsoleApp` project.
3. Type your commands and start mining! 🚀

---
**Made with ❤️ by Amir & Moein**

🔗 **Check us out on GitHub:**
*   👤 [Amir (ahk1384)](https://github.com/ahk1384)
*   👤 [Moein (MoeinH-0)](https://github.com/MoeinH-0)
