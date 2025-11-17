<script setup lang="ts">
import { reactive, ref } from 'vue';
import { SubmissionApi } from '@/api/mock-submission-api';

interface ClientForm {
    firstName: string
    lastName: string
    birthDate: string
    gender: 'male' | 'female' | 'other'
    email: string
    phone: string
    address: string
}

const form = reactive<ClientForm>({
    firstName: '',
    lastName: '',
    birthDate: '',
    gender: 'male',
    email: '',
    phone: '',
    address: '',
})

const errors = reactive<Record<string, string>>({})
const status = ref('')

function validateForm(): boolean {
    errors.firstName = form.firstName ? '' : 'Enter first name'
    errors.lastName = form.lastName ? '' : 'Enter last name'
    errors.birthDate = form.birthDate ? '' : 'Choose birth date'
    errors.gender = form.gender ? '' : 'Choose gender'
    errors.email = /\S+@\S+\.\S+/.test(form.email) ? '' : 'Enter correct email'
    errors.phone = form.phone.length >= 6 ? '' : 'Enter correct phone'
    errors.address = form.address ? '' : 'Enter address'

    return Object.values(errors).every(e => !e)
}

async function submitForm() {
    if (!validateForm()) {
        console.error("form is not valid", );
        return;
    }
    
    try {
        const result = await SubmissionApi.createClient({...form});
        console.log('New client successfully created:', result);
        status.value = 'New client successfully created!';
        // clean up form
        resetForm();
    } catch(error: any) {
        console.error(error);
        status.value = error.message;
    }
}

function resetForm() {
  form.firstName = ''
  form.lastName = ''
  form.birthDate = ''
  form.gender = 'male' 
  form.email = ''
  form.phone = ''
  form.address = ''
}
</script>

<template>
    <form @submit.prevent="submitForm" class="form">
        <label>
            First Name:
            <input v-model="form.firstName" type="text" required/>
            <span class="error">{{ errors.firstName }}</span>
        </label>

        <label>
            Last Name:
            <input v-model="form.lastName" type="text" required/>
            <span class="error">{{ errors.lastName }}</span>
        </label>

        <label>
            Birth Date:
            <input v-model="form.birthDate" type="date" required/>
            <span class="error">{{ errors.birthDate }}</span>
        </label>

        <fieldset>
            <legend>Gender</legend>
            <label>
                <input type="radio" value="male" v-model="form.gender" />
                Male
            </label>
            <label>
                <input type="radio" value="female" v-model="form.gender" />
                Female
            </label>
            <label>
                <input type="radio" value="other" v-model="form.gender" />
                Other
            </label>
            <span class="error">{{ errors.gender }}</span>
        </fieldset>

        <label>
            Address:
            <input v-model="form.address" type="text" />
            <span class="error">{{ errors.address }}</span>
        </label>

        <label>
            Email:
            <input v-model="form.email" type="email" />
            <span class="error">{{ errors.email }}</span>
        </label>

        <label>
            Phone number:
            <input v-model="form.phone" type="text" />
            <span class="error">{{ errors.phone }}</span>
        </label>

        <button type="submit">Create client</button>
        <div v-if="status" class="status">{{ status }}</div>
    </form>
</template>


<style scoped>
.form {
  max-width: 450px;
  display: flex;
  flex-direction: column;
  gap: 12px;
}
fieldset {
  border: 1px solid #ccc;
  padding: 10px;
}
.error {
  color: red;
  font-size: 0.9em;
}
.status {
  margin-top: 10px;
  color: green;
}
</style>