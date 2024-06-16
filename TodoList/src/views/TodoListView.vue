<template>
  <div class="content">
    <section class="column1"></section>
    <section class="column2">
      <box-shadow class="box">
        <h2 class="header">
          <v-icon class="icon" name="co-list-numbered-rtl" scale="1.5" />
          My Todo Lists
        </h2>
        <div class="filters">
          <label for="incomplete">
            <input
              type="checkbox"
              id="incomplete"
              v-model="filterIncomplete"
              value="0"
            />
            Incomplete
          </label>
          <label for="complete">
            <input
              type="checkbox"
              id="complete"
              v-model="filterComplete"
              value="1"
            />
            Complete
          </label>
        </div>
      </box-shadow>
      <todo-list-table :filterStatus="filterStatus"></todo-list-table>
    </section>
    <section class="column3"></section>
  </div>
</template>

<script>
import TodoListTable from "../components/TodoList/TodoListTable.vue";
export default {
  components: {
    TodoListTable,
  },
  data() {
    return {
      filterIncomplete: true,
      filterComplete: true,
    };
  },
  computed: {
    filterStatus() {
      if (this.filterIncomplete && this.filterComplete) {
        return null; // Show both
      } else if (this.filterComplete) {
        return 1; // Show complete
      } else if (this.filterIncomplete) {
        return 0; // Show complete
      } else {
        return -1;
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



</style>
