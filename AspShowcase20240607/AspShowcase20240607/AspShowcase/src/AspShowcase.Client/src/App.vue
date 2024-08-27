<script setup>
import { RouterLink, RouterView } from 'vue-router'
import axios from 'axios';

</script>

<template>
  <div class="appView">
    <nav>
      <div><RouterLink to="/">Teams</RouterLink></div>
      <div><RouterLink to="/pupils">Pupils</RouterLink></div>
    </nav>
    <div class="content">
      <RouterView />
    </div>
  </div>
</template>

<script>
export default {
  name: 'App',
  data: function () {
    return {
      teams: []
    }
  },
  async mounted() {
    // Durch axios.baseUrl wird der Pfad /api und bei Bedarf https://localhost:5000 
    // automatisch vorangestellt
    const response = await axios.get('teams');
    this.teams = response.data;
  },
  methods: {

  }
}
</script>

<style scoped>
.appView {
  display: flex;
  flex-direction: column;
}
nav {
  background-color: khaki;
  border: 3px solid hotpink;
  border-radius: 5px;
  padding: 1em;
  display: flex;
  column-gap: 1em;
}
nav a {
  text-decoration: none;
  color: black;
  border:1px solid black;
  padding: 0.25em;
  border-radius: 5px;
}

</style>
