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
    itemsPerPage: 10,
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
    <div class="gap-2 overflow-x-hidden p-4 space-y-4">
       <h1 class="text-3xl font-bold text-gray-600 text-center">Submissions</h1>

       <FormInput label="" v-model="filter.query" placeholder="Search submissions..." />
  
       <Pager 
            v-model="filter.page" 
            :total-pages="totalPages" 
            @update:model-value="load"
        />

        <div v-if="loading" class="flex justify-center py-10 ">
            <div class="animate-spin rounded-full h-10 w-10 border-4 border-gray-300 border-t-blue-500"></div>
        </div>

        <div v-else class="flex flex-col gap-8 justify-center">
            <div v-for="s in submissions" :key="s.id" 
                class="rounded-sm border-gray-300 shadow-sm p-4 hover:shadow-md transition w-full"
            >
                <h4 class="text-sm font-bold text-gray-600 mb-2">
                   Submitted on {{ s.createdOnUtc.toLocaleString() }}
                </h4>
                <pre class="text-xs bg-gray-50 p-2 rounded overflow-x-auto font-mono">{{ formatJson(s.payload) }}</pre>
                <br>
                <h4 class="lowercase font-bold text-gray-500 text-sm mb-2">{{ s.formId }} | {{ s.formName }}</h4>
            </div>
        </div>
    </div>
</template>