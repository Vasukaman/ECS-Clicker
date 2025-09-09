# ECS Clicker Tycoon Prototype

This project is a simple idle clicker/tycoon prototype built from the ground up using a pure **Entity Component System (ECS)** architecture with the **LeoEcsLite** framework. It was originally created as a technical test and has since been refined as a portfolio piece to demonstrate clean, data-driven, and event-driven design patterns in Unity.

---

### Key Features & Architecture

The project is built around a clean, logical data pipeline that emphasizes the separation of concerns.

* **Pure ECS Architecture:** All game logic is handled by systems operating on component data, using the high-performance **LeoEcsLite** library without any extensions.
* **Event-Driven Design:** Systems are decoupled and communicate through short-lived **event components**. This creates a clean and scalable message bus for game events (e.g., `RevenueCollectedEvent`, `UpgradeRequest`).
* **Data-Driven Approach:** All game balance and text are driven by **ScriptableObjects**, allowing for easy modification without changing code.
    * **GameConfig:** Contains all numerical data (costs, income multipliers, delays).
    * **NamesConfig:** Contains all display text, demonstrating a localization-ready architecture.
* **Robust Data Persistence:** The entire game state, including player balance and the income progress of each business, is saved to and loaded from **PlayerPrefs**.
* **Microsoft C# Code Style:** The codebase adheres to standard Microsoft C# naming and layout conventions for clarity and maintainability.

---

### Editor Tools

To improve the development and testing workflow, several custom editor tools were created. These allow for quick iteration and debugging without needing to play through the game manually.

*(Here you can add a GIF showcasing you using the menu)*

**[Image or GIF of the "Tools > Save Data" menu in action]**

The tools, accessible from the **Tools > Save Data** menu, provide functionality for:
* Clearing all saved data to test a fresh install.
* Resetting player balance or business progress independently.
* Applying a "cheat" save with maximum money for easy testing of late-game mechanics.Of course. It's an excellent idea to turn this into a portfolio piece. The feedback you received, even though it led to a rejection, is incredibly valuable for learning and improving. Let's create a strong, professional README that accurately describes the project and its architecture for your portfolio.

Here is a clean, professional `README.md` file you can use for your GitHub repository.

***

# ECS Clicker Tycoon Prototype

This project is a simple idle clicker/tycoon prototype built from the ground up using a pure **Entity Component System (ECS)** architecture with the **LeoEcsLite** framework. It was originally created as a technical test and has since been refined as a portfolio piece to demonstrate clean, data-driven, and event-driven design patterns in Unity.

---

### Key Features & Architecture

The project is built around a clean, logical data pipeline that emphasizes the separation of concerns.

* **Pure ECS Architecture:** All game logic is handled by systems operating on component data, using the high-performance **LeoEcsLite** library without any extensions.
* **Event-Driven Design:** Systems are decoupled and communicate through short-lived **event components**. This creates a clean and scalable message bus for game events (e.g., `RevenueCollectedEvent`, `UpgradeRequest`).
* **Data-Driven Approach:** All game balance and text are driven by **ScriptableObjects**, allowing for easy modification without changing code.
    * **GameConfig:** Contains all numerical data (costs, income multipliers, delays).
    * **NamesConfig:** Contains all display text, demonstrating a localization-ready architecture.
* **Robust Data Persistence:** The entire game state, including player balance and the income progress of each business, is saved to and loaded from **PlayerPrefs**.
* **Microsoft C# Code Style:** The codebase adheres to standard Microsoft C# naming and layout conventions for clarity and maintainability.

---

### Editor Tools

To improve the development and testing workflow, several custom editor tools were created. These allow for quick iteration and debugging without needing to play through the game manually.

The tools, accessible from the **Tools > Save Data** menu, provide functionality for:
* Clearing all saved data to test a fresh install.
* Resetting player balance or business progress independently.
* Applying a "cheat" save with maximum money for easy testing of late-game mechanics.

<img width="727" height="151" alt="image" src="https://github.com/user-attachments/assets/a7765891-db46-4372-8777-a139bc827710" />
