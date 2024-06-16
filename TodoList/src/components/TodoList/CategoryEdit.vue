<template>
  <div v-if="show" class="modal">
    <div class="modal-content">
      <h2>Edit Category</h2>
      <form @submit.prevent="cetegoryItem">
        <div class="form-group">
          <label for="categoryName">Category Name:</label>
          <input type="text" id="categoryName" v-model="cetegoryItem.name" required />
          <span v-if="validationError" class="error-message">{{ validationError }}</span>
        </div>
        <div class="action">
          <button type="submit" class="cancel-button" @click="$emit('close')">
            Cancel
          </button>
          <button type="submit" class="save-button">Save</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
export default {
  inject:["axios"],
  props: {
    show: {
      type: Boolean,
      required: true,
    },
    cetegoryItem:{
      type: Object,
      required: true
    }
  },
  data() {
    return {
      validationError: null,
    };
  },
  methods: {
    async editCategory(cetegoryItem) {
      this.validationError = null; 

      if (cetegoryItem.name.trim() == "") {
        this.validationError = "Category name is required";
        return;
      }

      try {
        const response = await this.axios.put("/Category", cetegoryItem);
        console.log(response);
        if (response.status == 200){
          this.$router.go(0)
        }
      } catch (error) {
        console.error(error);
        alert(error.response.request.responseText)
      }
    },
  },
};
</script>

<style scoped>
.error-message {
  color: red;
  font-size: 12px;
}
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
  align-content: end;
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
  background-color: var(--cyan-500); /* Green color for "add" button */
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
</style>
