<script setup lang="ts">
import { ref, computed } from 'vue'
import { useStore } from '../Store'

const store = useStore()
const isTracking = ref(false)
const heartRate = ref(0)
const duration = ref(0)
const heartRates = ref<number[]>([])
const maxHeartRate = ref(0)
const minHeartRate = ref(Infinity)
const avgHeartRate = ref(0)
let interval: number | undefined

// Define heart rate zones
const heartRateZones = [
  { zone: 0, min: 45, max: 92, label: 'Resting 😴' },
  { zone: 1, min: 92, max: 109, label: 'Zone1 😃' },
  { zone: 2, min: 110, max: 127, label: 'Zone 2 😅' },
  { zone: 3, min: 128, max: 146, label: 'Zone 3 😬' },
  { zone: 4, min: 147, max: 164, label: 'Zone 4 😮‍💨' },
  { zone: 5, min: 165, max: 220, label: 'Zone 5 🥵' },
]

// Computed property to determine the current heart rate zone
const currentHeartRateZone = computed(() => {
  const zone = heartRateZones.find(z => heartRate.value >= z.min && heartRate.value <= z.max)
  return zone ? zone.label : '💀'
})

const startActivity = () => {
  isTracking.value = true
  const activity = store.startActivity()
  duration.value = 0
  heartRates.value = []
  maxHeartRate.value = 0
  minHeartRate.value = Infinity
  avgHeartRate.value = 0

  interval = setInterval(() => {
    if (!isTracking.value) {
      clearInterval(interval)
      store.endActivity(activity)
      return
    }
    duration.value += 1
  }, 1000)
}

const handleHeartRateMessage = (event: any) => {
  if (!event || !event.detail || !event.detail.message) {
    console.error('No heart rate data received. Received:', event)
    return
  }
  const newHeartRate = Number(event.detail.message)
  updateHeartRate(newHeartRate);
};

// Make the method global by attaching it to the window object
const updateHeartRate = (newHeartRate: number) => {
  heartRate.value = newHeartRate
  const activity = store.state.activities[store.state.activities.length - 1]
  if(!activity || activity.endTime){
    return
  }
  heartRates.value.push(heartRate.value)
  maxHeartRate.value = Math.max(...heartRates.value)
  minHeartRate.value = Math.min(...heartRates.value)
  avgHeartRate.value = heartRates.value.reduce((a, b) => a + b, 0) / heartRates.value.length
  store.addActivityData(activity, heartRate.value)
}

// Make the method global by attaching it to the window object
(window as any).updateHeartRate = handleHeartRateMessage

window.addEventListener('HybridWebViewMessageReceived', handleHeartRateMessage);

const stopActivity = () => {
  isTracking.value = false
  const activity = store.state.activities[store.state.activities.length - 1]
  store.endActivity(activity)
}

const formatDuration = (duration: number) => {
  const hours = Math.floor(duration / 3600)
  const minutes = Math.floor(duration / 60)
  const seconds = duration % 60
  let durationString = ''
  if (hours > 0) {
    durationString += `${hours}:`
  }
  if (minutes > 0) {
    durationString += `${hours > 0 ? '0':''}${minutes}:`
  }
  durationString += `${seconds < 10 && minutes > 0 ? '0' : ''}${seconds}`
  return durationString
}

</script>

<template>
  <div>
    <button @click="startActivity" v-if="!isTracking">Start Activity</button>
    <button @click="stopActivity" v-if="isTracking">Stop Activity</button>
    <div v-if="isTracking">
      <p>
      Current Heart Rate: {{ heartRate }} 
      <span class="heart">❤️</span>
      </p>
      <p>Duration: {{ formatDuration(duration) }}</p>
      <p>Heart Rate Zone: {{ currentHeartRateZone }}</p>
    </div>
  </div>
</template>

<style scoped>
.heart {
  display: inline-block;
  animation: blink 1s infinite;
}

@keyframes blink {
  0%, 100% { opacity: 1; }
  50% { opacity: 0; }
}
</style>