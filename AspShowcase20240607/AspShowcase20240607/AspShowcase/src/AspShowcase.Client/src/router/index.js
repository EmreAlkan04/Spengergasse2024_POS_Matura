import { createRouter, createWebHistory } from 'vue-router'
import TeamsView from '../views/TeamsView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: TeamsView
    },
    {
      path: '/teams/:teamGuid',
      name: 'teamDetail',
      component: () => import('../views/TeamDetailsView.vue')
    },
    {
      path: '/pupils',
      name: 'pupils',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/PupilsView.vue')
    }
  ]
})

export default router
