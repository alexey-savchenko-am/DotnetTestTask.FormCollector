<script setup lang="ts">
import {ref, reactive, onMounted, watch, computed} from 'vue';
import { submissionApi } from '@/api';
import type { Submission } from '@/types/Submission';
import type { SubmissionFilter } from '@/types/SubmissionFilter';
import type { SubmissionSearchResult } from '@/types/SubmissionSearchResult';
import Pager from '@/components/Pager.vue';
import FormInput from '@/components/FormInput.vue';

const submissions = ref<Submission[]>([])
const total = ref(0);
const loading = ref(false);

const filter = reactive<SubmissionFilter>({
    page: 1,
    itemsPerPage: 5,
    query: null
});

let debounceTimer: number | undefined;

watch(
  () => filter.query,
  () => {
    filter.page = 1 

    if (debounceTimer) clearTimeout(debounceTimer);

    debounceTimer = window.setTimeout(() => {
        load()
    }, 500);
  }
)

const totalPages = computed(() => {
  return Math.ceil(total.value / filter.itemsPerPage);
});

async function load() {
    loading.value = true;
    const result: SubmissionSearchResult = await submissionApi.getSubmissions(filter);
    console.log(result);
    submissions.value = result.submissions;
    total.value = result.totalCount;
    loading.value = false;
}

function formatJson(json: string): string{
    return JSON.stringify(JSON.parse(json), null, 2);
}


onMounted(load);
</script>

<template>
    <div class="p-6 space-y-6 max-w-3xl mx-auto">
      <h1 class="text-4xl font-extrabold text-gray-700 text-center">Submissions</h1>
  
      <FormInput 
        label="" 
        v-model="filter.query" 
        placeholder="Search submissions..." 
        class="w-full"
      />
  
      <Pager 
        v-model="filter.page" 
        :total-pages="totalPages" 
        @update:model-value="load"
        class="w-full"
      />
  
      <div v-if="loading" class="flex justify-center py-12">
        <div class="animate-spin rounded-full h-12 w-12 border-4 border-gray-300 border-t-blue-500"></div>
      </div>
  
      <div v-else class="flex flex-col gap-6">
        <div 
          v-for="s in submissions" 
          :key="s.id"
          class="bg-white rounded-xl shadow hover:shadow-lg transition p-5 flex flex-col"
        >
          <div class="mb-3">
            <h4 class="text-gray-500 text-sm mb-1">Submitted on</h4>
            <p class="text-gray-700 font-medium">{{ s.createdOnUtc }}</p>
          </div>
  
          <pre class="bg-gray-50 p-3 rounded text-xs overflow-x-auto font-mono mb-3">{{ formatJson(s.payload) }}</pre>
  
          <div class="mt-auto">
            <h4 class="text-blue-600 font-semibold text-sm truncate">{{ s.formName }}</h4>
            <p class="text-gray-400 text-xs">{{ s.formId }}</p>
          </div>
        </div>
      </div>
    </div>
</template>
  