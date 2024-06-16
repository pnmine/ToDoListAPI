import { createRouter, createWebHistory } from 'vue-router'
import TodoListView from '../views/TodoListView.vue'
import CategoryView from '../views/CategoryView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'todoListView',
      component: TodoListView
    },
    {
      path: '/category',
      name: 'categoryView',
      component: CategoryView
    },
  ]
})

export default router
