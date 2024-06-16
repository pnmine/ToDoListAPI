<template>
  <slot>
    <table class="styled-table">
      <thead>
        <tr>
          <th class="name-cell">Name</th>
          <th class="date-cell">Start Date</th>
          <th class="date-cell">End Date</th>
          <th class="status-cell">Status</th>
          <th class="category-cell">Category</th>
          <th class="action-cell"></th>
          <th class="action-cell"></th>
        </tr>
      </thead>
      <tbody v-for="todo in filteredTodoLists" :key="todo.Id">
        <tr>
          <td>{{ todo.name }}</td>
          <td>{{ formatDate(todo.startDate) }}</td>
          <td>{{ formatDate(todo.endDate) }}</td>
          <td>{{ formatStatus(todo.status) }}</td>
          <td>{{ todo.category.name }}</td>
          <td>
            <button class="button" @click="openEditModal(todo)">Edit</button>
          </td>
          <td>
            <button class="button" @click="deleteItem(todo)">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
    <div class="button-container">
      <button @click="openCreateModal" class="create-todo-button">
        Create Todo
      </button>
    </div>
    <create-todo-modal
      :show="showCreateModal"
      @close="closeCreateModal"
      @save="save"
    />

    <edit-todo-modal
      :show="showEditModal"
      :editItem="editedItem"
      @close="closeEditModal"
      @save="save"
    />
  </slot>
</template>
<script>
import EditTodoModal from "./TodoListEdit.vue";
import CreateTodoModal from "./TodoListCreate.vue";
export default {
  components: {
    EditTodoModal,
    CreateTodoModal,
  },
  inject: ["axios"],
  props: ["filterStatus"],
  data() {
    return {
      allTodoLists: [],
      showEditModal: false,
      showCreateModal: false,
      todoItemId: null,
    };
  },
  mounted() {
    this.fetchTodoItems();
  },
  computed: {
    filteredTodoLists() {
      if (this.filterStatus == null) {
        return this.allTodoLists; // Show all tasks
      } else if (this.filterStatus == -1) {
        return;
      }
      return this.allTodoLists.filter(
        (todo) => todo.status == this.filterStatus
      );
    },
  },
  methods: {
    async fetchTodoItems() {
      try {
        const response = await this.axios.get("/todolist");
        this.allTodoLists = response.data;
        console.log("this.allTodoLists", this.allTodoLists);
      } catch (error) {
        console.error(error);
      }
    },
    formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString() + " " + date.toLocaleTimeString();
    },
    formatStatus(status) {
      if (status == 0) {
        return "Incomplete";
      } else {
        return "Complete";
      }
    },
    openEditModal(todo) {
      console.log("Edit");
      this.editedItem = { ...todo };
      console.log(this.editedItem);
      this.showEditModal = true;
    },
    closeEditModal() {
      this.showEditModal = false;
      this.editedItem = null;
    },
    async deleteItem(todo) {
      console.log("delete", todo);
      const isConfirm = confirm("Are you sure?");
      if (isConfirm) {
        try {
          await this.axios.delete("/todolist/" + todo.id).then((response) => {
            if (response.status == 200) {
              console.log(response.data);
              this.$router.go(0); // or fetch data
            }
          });
        } catch (error) {
          console.log(error);
        }
      }
    },
    async save(todoItem) {
      console.log(todoItem);
      this.todoItemId = todoItem.id;
      if (this.todoItemId != null) {
        //editmode
        const data = {
          id: this.todoItemId,
          name: todoItem.name,
          startDate: todoItem.startDate,
          endDate: todoItem.endDate,
          status: todoItem.status,
          category: { name:todoItem.category.name},
        };

        try {
          const response = await this.axios.put("/todolist", data);
          if (response.status == 200) {
            console.log("Edit Success", response.data);
            this.todoItemId = null;
            this.closeEditModal();
            this.$router.go(0); // or fetch data
          }
        } catch (error) {}
      } else {
        console.log("Create mode");
        //create
        try {
          const data = {
            name: todoItem.name,
            startDate: todoItem.startDate,
            endDate: todoItem.endDate,
            status: todoItem.status,
            category: {name:todoItem.category},
          };
          console.log("create Data",data);
          const response = await this.axios.post("/todolist/create", data);
          if (response.status == 200) {
            console.log(response.data);
            this.closeCreateModal();
            this.$router.go(0);
          }
        } catch (error) {
          console.error(error);
        }
      }
    },
    openCreateModal() {
      console.log("Create");
      this.showCreateModal = true;
    },
    closeCreateModal() {
      this.showCreateModal = false;
    },
    async createTodo(newTodo) {},
    formatDateTimeLocal(dateString) {
      const date = new Date(dateString);
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, "0"); // Months are 0-indexed
      const day = String(date.getDate()).padStart(2, "0");
      const hours = String(date.getHours()).padStart(2, "0");
      const minutes = String(date.getMinutes()).padStart(2, "0");

      return `${year}-${month}-${day}T${hours}:${minutes}`;
    },
  },
};
</script>
<style scoped>
.styled-table {
  border-spacing: 1;
  border-collapse: collapse;
  background: var(--grey-050);
  border-radius: 12px;
  overflow: hidden;
  max-width: 100%;
  width: 100%;
  margin: 0 auto;
  position: relative;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.15);
}
.styled-table thead tr {
  background-color: var(--grey-500);
  color: #ffffff;
  text-align: left;
}
.styled-table th {
  font-family: "Kanit", sans-serif;
  font-weight: 400;
  font-style: normal;
}
.styled-table th,
.styled-table td {
  padding: 12px 15px;
}
.styled-table tbody tr {
  border-bottom: 1px solid #dddddd;
}

.styled-table tbody tr:nth-of-type(even) {
  background-color: #f3f3f3;
}

.styled-table tbody tr.active-row {
  font-weight: bold;
  color: #009879;
}

.button-container {
  display: flex;
  justify-content: flex-end;
  margin-top: 10px;
}
.create-todo-button {
  background-color: var(--grey-700);
  color: white;
  padding: 10px 15px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.button {
  background-color: var(--grey-500); /* Example color */
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
.button:hover {
  background-color: var(--grey-700);
}

/* Table column sizing
================================== */
.name-cell {
  width: 45%;
}
.date-cell {
  width: 10%;
}

.status-cell {
  width: 10%;
}
.category-cell {
  width: 20%;
}
.action-cell {
  width: 2.5%;
}
</style>
