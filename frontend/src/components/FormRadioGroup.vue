<script setup lang="ts">
const props = defineProps<{
  label: string
  modelValue: string
  options: { label: string; value: string }[]
  error?: string
}>()

const emit = defineEmits(['update:modelValue'])

function onSelect(value: string) {
  emit('update:modelValue', value)
}
</script>

<template>
  <fieldset class="flex flex-col gap-2 border border-gray-300 rounded-lg p-4">
    <legend class="font-medium text-gray-700 mb-1 text-sm md:text-base">{{ label }}</legend>

    <div
      v-for="option in options"
      :key="option.value"
      class="flex items-center gap-2"
    >
      <label class="flex items-center gap-4 cursor-pointer text-gray-800 text-sm md:text-base">
        <input
          type="radio"
          :value="option.value"
          :checked="modelValue === option.value"
          @change="onSelect(option.value)"
          class="
            w-4 h-4 text-blue-600
            border border-gray-300 rounded
            outline-none
            hover:border-blue-400
            active:scale-95
            transition-all duration-150
          "
        />
        {{ option.label }}
      </label>
    </div>

    <p v-if="error" class="text-red-500 text-sm mt-1 min-h-[1.25rem]">{{ error }}</p>
  </fieldset>
</template>

<style scoped>
</style>
