<template>
  <div class="content">
    <section class="column1"></section>
    <section class="column2">
      <box-shadow class="box">
        <h2 class="header">All Category</h2>
        <ul v-for="category in categoryList" :key="category.id">
          <div class="list">
            {{ category.name }}
            <div class="actions">
              <button class="button" @click="openEditModal(category)">
                Edit
              </button>
              <button class="button" @click="deleteItem(category.id)">
                Delete
              </button>
            </div>
          </div>
        </ul>
      </box-shadow>
      <div class="button-container">
        <button
          type="button"
          class="add-category-button"
          @click="showAddCategoryModal = true"
        >
          Add Category
        </button>
      </div>
    </section>
    <section class="column3"></section>
  </div>
  <category-create
    :show="showAddCategoryModal"
    @category-added="addCategory"
    @close="showAddCategoryModal = false"
  />
  <category-edit
    :show="showEditModal"
    :cetegoryItem="editCategory"
    @close="closeEditModal"
  />
</template>

<script>
import CategoryCreate from "../components/TodoList/CategoryCreate.vue";
import CategoryEdit from "../components/TodoList/CategoryEdit.vue";
export default {
  inject: ["axios"],
  components: {
    CategoryCreate,
    CategoryEdit,
  },
  data() {
    return {
      showAddCategoryModal: false,
      showEditCategoryModal: false,
      editCategory: null,
      categoryList: [],
    };
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
    openEditModal(editCate) {
      console.log("Edit");
      this.editCategory = { ...editCate };
      console.log(this.editCategory);
      this.showEditModal = true;
    },
    closeEditModal() {
      this.showEditModal = false;
      this.editCategory = null;
    },
    addCategory() {
      this.fetchCategory();
    },
    async deleteItem(id) {
      console.log("delete", id);
      const isConfirm = confirm("Are you sure?");
      if (isConfirm) {
        try {
          await this.axios.delete("/category/" + id).then((response) => {
            console.log(response);
            if (response.status == 200) {
              console.log(response.data);
              this.$router.go(0); 
            }
          });
        } catch (error) {
          console.log(error);
          alert(error.response.data)
        }
      }
    },
  },
};
</script>
<style scoped>
.content {
  margin: auto;
  display: grid;
  grid-template-columns: 1fr 3fr 1fr;
}
.box {
  padding-bottom: 1rem !important ;
}
section.column2 {
  text-align: left;
}
h2.header {
  margin-top: 0;
  display: flex;
  align-items: center;
}
h2 .icon {
  margin-right: 10px;
}

.controls {
  display: box;
}

ul {
  margin: 0;
  padding: 10px;
}
.button-container {
  display: flex;
  justify-content: flex-end;
  margin-top: 10px;
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
.add-category-button:hover {
  background-color: var(--grey-700);
}
.actions {
  display: flex;
}
.list {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
ul:nth-child(odd) {
  border: 1px solid var(--grey-100);
  border-radius: 6px;
}

.button {
  background-color: var(--grey-500); /* Example color */
  color: white;
  padding: 5px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  margin: 2px;
}
.button:hover {
  background-color: var(--grey-700);
}
</style>
