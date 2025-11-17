<script setup lang="ts">
import { defineProps } from 'vue';

const props = defineProps<{
    label: string
    modelValue: string
    type?: string
    error?: string
}>()

const emit = defineEmits(['update:modelValue'])

function onInput(e: Event) {
  emit('update:modelValue', (e.target as HTMLInputElement).value)
}
</script>

<template>
  <div class="flex flex-col">

    <div class="flex flex-row justify-between">
        <label class="text-sm md:text-base font-medium text-gray-700 mb-1">
            {{ label }}
        </label>
        <span v-if="error" class="text-red-500 text-sm mt-1 min-h-[1.25rem] lowercase">
            {{ error }}
        </span>
    </div>


    <input
        :type="type ?? 'text'"
        :value="modelValue"
        @input="onInput"
        class="
            border border-gray-300 rounded-lg px-3 py-2
            outline-0
            hover:border-blue-400
            focus:border-blue-200 focus:ring-2 focus:ring-blue-300 focus:ring-opacity-50
            active:border-blue-600 active:ring-0
            transition-all duration-150
        "
        />
  </div>
</template>
