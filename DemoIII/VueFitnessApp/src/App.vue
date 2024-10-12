<script setup lang="ts">
import ActivityList from './components/ActivityList.vue'
import StartActivity from './components/StartActivity.vue'
</script>

<template>
  <div>
    <h1>Fitness App</h1>
    <StartActivity />
    <ActivityList />
  </div>
</template>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
/* Add your styles here */
</style>
