<script setup lang="ts">
import { computed } from 'vue'
import { useStore, Activity } from '../Store'

// Utility function to format duration
function formatDuration(start: number, end: number|any): string {
  const duration = (end ?? new Date().getTime()) - start
  const seconds = Math.floor((duration / 1000) % 60)
  const minutes = Math.floor((duration / (1000 * 60)) % 60)
  const hours = Math.floor((duration / (1000 * 60 * 60)) % 24)

  const hoursStr = hours < 10 ? `0${hours}` : hours
  const minutesStr = minutes < 10 ? `0${minutes}` : minutes
  const secondsStr = seconds < 10 ? `0${seconds}` : seconds

  return `${hoursStr}:${minutesStr}:${secondsStr}`
};

const formatDate = (timestamp: number) => {
  const date = new Date(timestamp);
  return date.toLocaleDateString('en-GB', {
    day: '2-digit',
    month: 'long',
    year: 'numeric',
  }).replace(/(\d{2}) (.+) (\d{4})/, '$1. $2 $3');
};

const store = useStore()
const activities = computed(() => store.state.activities.sort((a: Activity, b: Activity) => b.startTime - a.startTime))

const resetHistory = () => {
  store.resetHistory()
}
</script>

<template>
  <div>
    <h2>Past Workouts</h2>
    <button @click="resetHistory">Reset History</button>
    <ul>
      <li v-for="activity in activities" :key="activity.startTime">
        <p>Date: {{ formatDate(activity.startTime) }} - Duration: {{ formatDuration(activity.startTime, activity.endTime) }}</p>
      </li>
    </ul>
  </div>
</template>