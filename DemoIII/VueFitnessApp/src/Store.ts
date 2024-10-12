import { reactive, watch } from 'vue'

export interface Activity {
  startTime: number
  endTime: number | null
  data: { time: number, gpsPosition: [number, number], heartRate: number }[]
}

const loadState = (): { activities: Activity[] } => {
  const savedState = localStorage.getItem('activityState')
  return savedState ? JSON.parse(savedState) : { activities: [] }
}

const state = reactive(loadState())

const saveState = () => {
  localStorage.setItem('activityState', JSON.stringify(state))
}

// Watch for changes in the state and save to localStorage
watch(state, saveState, { deep: true })

const generateStubActivities = () => {
  const now = Date.now()
  const oneDay = 24 * 60 * 60 * 1000

  for (let i = 1; i <= 3; i++) {
    const startTime = now - i * 3 * oneDay
    const endTime = startTime + (Math.random() * 60 + 30) * 60 * 1000;
    state.activities.push({
      startTime,
      endTime,
      data: []
    })
  }
}

const resetHistory = () => {
  state.activities.length = 0 // Clear the current activities
  generateStubActivities() // Regenerate the initial stub activities
  saveState() // Save the state to localStorage
}

if (state.activities.length === 0) {
  generateStubActivities()
}

const startActivity = (): Activity => {
  const activity: Activity = {
    startTime: Date.now(),
    endTime: null,
    data: []
  }
  state.activities.push(activity)
  console.log('Activity started:', activity)
  return activity
}

const endActivity = (activity: Activity) => {
  activity.endTime = Date.now()
  console.log('Activity ended:', activity)
  saveState()
}

const addActivityData = (activity: Activity, heartRate: number) => {
  activity.data.push({
    time: Date.now(),
    gpsPosition: [0, 0], // Stub GPS position
    heartRate
  })
}

const getPastWorkouts = () => {
  return state.activities
}

export const useStore = () => {
  return {
    state,
    startActivity,
    endActivity,
    addActivityData,
    getPastWorkouts,
    resetHistory
  }
}