<script setup lang="ts">
import { reactive, ref } from 'vue';
import { submissionApi } from '@/api';
import FormInput from './FormInput.vue';
import FormRadioGroup from './FormRadioGroup.vue';

interface ClientForm {
    firstName: string
    lastName: string
    birthDate: string
    gender: 'male' | 'female' | 'other'
    email: string
    phone: string
    address: string,
    contactMethod: 'Email' | 'Phone' | 'SMS',
    subscribeOnUpdates: boolean,
    agreeTerms: boolean
}

const form = reactive<ClientForm>({
    firstName: '',
    lastName: '',
    birthDate: '',
    gender: 'male',
    email: '',
    phone: '',
    address: '',
    contactMethod: 'Email',
    subscribeOnUpdates: false,
    agreeTerms: false
})

const errors = reactive<Record<string, string>>({})
const status = ref('')

function validateForm(): boolean {
    errors.firstName = form.firstName ? '' : 'Field first name is required'
    errors.lastName = form.lastName ? '' : 'Field last name is required'
    errors.birthDate = form.birthDate ? '' : 'Choose birth date'
    errors.gender = form.gender ? '' : 'Choose gender'
    errors.email = /\S+@\S+\.\S+/.test(form.email) ? '' : 'Enter correct email'
    errors.phone = form.phone.length >= 6 ? '' : 'Enter correct phone'
    errors.address = form.address ? '' : 'Field address is required'

    return Object.values(errors).every(e => !e)
}

async function submitForm() {

    if (!validateForm()) {
        console.error("Client form is not valid. Please fill all required fields.", );
        return;
    }
    
    try {
        const result = await submissionApi.createSubmission("client-form", "Create New Client", {...form});
        console.log('New client successfully created:', result);
        status.value = 'New client successfully created!';
        resetForm();
    } catch(error: any) {
        console.error(error);
        status.value = error.message;
    }
}

function resetForm() {
  form.firstName = '';
  form.lastName = '';
  form.birthDate = '';
  form.gender = 'male';
  form.email = '';
  form.phone = '';
  form.address = '';
  form.contactMethod = 'Email';
  form.subscribeOnUpdates = false;
  form.agreeTerms = false;
}
</script>

<template>
    <form @submit.prevent="submitForm" class="flex flex-col gap-2 px-2 w-full lg:w-[600px]">
        
        <FormInput label="First Name" v-model="form.firstName" :error="errors.firstName" />

        <FormInput label="Last Name" v-model="form.lastName" :error="errors.lastName" />

        <FormInput label="Birth Date" v-model="form.birthDate" :error="errors.birthDate" type="date"/>

        <FormRadioGroup
            label="Gender"
            v-model="form.gender"
            :options="[
                { label: 'Male', value: 'male' },
                { label: 'Female', value: 'female' },
                { label: 'Other', value: 'other' }
            ]"
            :error="errors.gender"
        />
        
        <FormInput label="Address" v-model="form.address" :error="errors.address" />

        <FormInput label="Email" v-model="form.email" :error="errors.email" type="email" />

        <FormInput label="Phone Number" v-model="form.phone" :error="errors.phone" />
        
        <label class="text-sm md:text-base font-medium text-gray-700 mb-1 flex flex-col gap-2">
            Preffered contact method:
            <select v-model="form.contactMethod" class=" border border-gray-300 rounded-lg px-3 py-2 outline-0">
                <option value="Email">Email</option>
                <option value="Phone">Phone</option>
                <option value="SMS">SMS</option>
            </select>
        </label>

        <label class="flex items-center gap-2 cursor-pointer text-gray-700">
            <input class="w-4 h-4" type="checkbox" v-model="form.subscribeOnUpdates"/>
            Subscribe on updates
        </label>

        <label class="flex items-center gap-2 cursor-pointer text-gray-700">
            <input class="w-4 h-4" type="checkbox" v-model="form.agreeTerms"/>
            Agree with terms
        </label>

        <button
            type="submit"
            class="
                w-full
                rounded-lg
                border-2 border-blue-300
                py-2 text-lg
                cursor-pointer
                transition-all
                transform
                active:scale-98
                active:border-blue-300
                hover:border-blue-300
                focus:outline-none
                focus:ring-2 focus:ring-blue-300 focus:ring-opacity-50
            "
        >
            Create Client
        </button>


        <div v-if="status" class="text-green-500">{{ status }}</div>
    </form>
</template>

