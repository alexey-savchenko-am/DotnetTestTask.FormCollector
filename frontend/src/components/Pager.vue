<script setup lang="ts">
import { defineProps, defineEmits, computed } from 'vue';

const props = defineProps<{
    totalPages: number,
    modelValue: number
}>();

const emit = defineEmits<{
    (e: 'update:modelValue', page: number): void
}>();

const visiblePages = computed(() => {
    const start = Math.max(1, props.modelValue - 2);
    const end = Math.min(props.totalPages, props.modelValue + 2);
    const pages = [];
    for (let i = start; i <= end; i++) {
        pages.push(i);
    }
    return pages;
});

function goToPage(page: number) {
    if (page < 1 || page > props.totalPages) return;
    emit('update:modelValue', page);
}

function prevPage() {
    goToPage(props.modelValue - 1);
}

function nextPage() {
    goToPage(props.modelValue + 1);
}
</script>

<template>
    <div v-if="totalPages > 1" class="flex gap-2 mt-4 text-xs md:text-base">
        <button
            class="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300 disabled:opacity-50"
            :disabled="modelValue === 1"
            @click="prevPage"
        >
            Prev
        </button>

        <button
            v-for="p in visiblePages"
            :key="p"
            class="px-3 py-1 rounded cursor-pointer"
            :class="p === modelValue ? 'bg-blue-500 text-white' : 'bg-gray-200 hover:bg-gray-300'"
            @click="goToPage(p)"
        >
            {{ p }}
        </button>

        <button
            class="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300 disabled:opacity-50"
            :disabled="modelValue === totalPages"
            @click="nextPage"
        >
            Next
        </button>
    </div>
</template>
