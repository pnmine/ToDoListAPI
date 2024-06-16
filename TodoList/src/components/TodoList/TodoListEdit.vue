<template>
  <div v-if="show" class="modal">
    <div class="modal-content">
      <h2>Edit Todo Item</h2>
      <form @submit.prevent="$emit('save', editItem)">
        <div class="form-group">
          <label for="editName">Name:</label>
          <input type="text" id="editName" v-model="editItem.name" />
        </div>
        <div class="form-group">
          <label for="editStartDate">Start Date:</label>
          <input
            type="datetime-local"
            id="editStartDate"
            v-model="formattedStartDate"
            @change="updateStartDate"
          />
        </div>
        <div class="form-group">
          <label for="editEndDate">End Date:</label>
          <input
            type="datetime-local"
            id="editEndDate"
            v-model="formattedEndDate"
            @change="updateEndDate"
          />
        </div>
        <div class="form-group">
          <label for="editStatus">Status:</label>
          <select id="editStatus" v-model="editItem.status">
            <option value="0">Incomplete</option>
            <option value="1">Complete</option>
          </select>
        </div>
        <div class="form-group">
          <label for="editCategory">Category:</label>
          <select id="editCategory" v-model="editItem.category.name">
            <option v-for="value in categoryList" :key="value.id" :value="value.name">
              {{ value.name }}
            </option>
          </select>
          <button type="button" class="add-category-button" @click="showAddCategoryModal = true">Add Category</button>
        </div>
        <div class="action">
          <button type="submit" class="cancel-button" @click="$emit('close')">
            Cancel
          </button>
          <button type="submit" class="save-button">Save</button>
        </div>
      </form>
      <category-create 
        :show="showAddCategoryModal"
        @category-added="addCategory" 
        @close="showAddCategoryModal = false" 
      />
    </div>
  </div>
</template>

<script>
import CategoryCreate from './CategoryCreate.vue';
export default {
  components: {
    CategoryCreate,
  },
  inject: ["axios"],
  props: {
    show: {
      type: Boolean,
      required: true,
    },
    editItem: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      showAddCategoryModal: false,
      categoryList: [],
    };
  },
  computed: {
    formattedStartDate() {
      return this.formatDateTimeLocal(this.editItem.startDate);
    },
    formattedEndDate() {
      return this.formatDateTimeLocal(this.editItem.endDate);
    },
  },
  mounted() {
    this.fetchCategory();
  },
  methods: {
    async fetchCategory() {
      try {
        const response = await this.axios.get("/category");
        this.categoryList = JSON.parse(JSON.stringify(response.data));
        console.log("this.categoryList", this.categoryList);
      } catch (error) {
        console.error(error);
      }
    },
    formatDateTimeLocal(dateString) {
      const date = new Date(dateString);
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, "0"); // Months are 0-indexed
      const day = String(date.getDate()).padStart(2, "0");
      const hours = String(date.getHours()).padStart(2, "0");
      const minutes = String(date.getMinutes()).padStart(2, "0");

      return `${year}-${month}-${day}T${hours}:${minutes}`;
    },
    updateStartDate(event) {
      this.editItem.startDate = event.target.value;
    },
    updateEndDate(event) {
      this.editItem.endDate = event.target.value;
    },
    addCategory(){
      this.fetchCategory()
    }
  },
};
</script>

<style scoped>
.modal-header {
  display: flex; /* Use flexbox for layout */
  justify-content: space-between; /* Align items to the edges */
  align-items: center; /* Vertically center items */
}

.modal-footer {
  display: flex;
  justify-content: flex-end; /* Align buttons to the right */
  margin-top: 20px;
}
.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000; /* Ensure modal is above other content */
}

.modal-content {
  display: block;
  background-color: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.26);
  width: 400px; /* Adjust width as needed */
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
}

.save-button {
  background-color: #007bff; /* Example color */
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
.cancel-button {
  background-color: var(--grey-300); /* Example color */
  color: white;
  padding: 10px 15px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
.action {
  display: flex;
  justify-content: space-between;
}

.add-category-button {
  margin-left: 5px;
  padding: 5px 10px;
  background-color: #28a745; /* Green color for "add" button */
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
</style>
