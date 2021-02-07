<template>
  <div>
      <Container>
        <Header />
      </Container>

      <Container>

        {{ this.state }}

        <ErrorAlert v-if="displayError" :error="error" />

        <PrizeDrawForm 
          v-if="displayForm" 
          v-bind:validationRules="this.validationRules"
          v-on:successfulSubmit="successfulPrizeDrawSubmit" />

        <Spinner v-if="displaySpinner"/>

        <div v-if="displayConfirmation" class="text-center">
          <h2>Entry Successful</h2>
          <p>Thank you for entering the Acme Corporation prize draw</p>
          <template v-if="submissions < config.maxSubmissions">
            <p>You can enter the prize draw a maximum number of {{ config.maxSubmissions }} times for every valid serial number that you have. Would you like to enter again?</p>
            <Button v-on:click="tryAgain" :text="'New Entry'" />
          </template>
        </div>
      </Container>
  </div>
</template>

<script>

import Header from '@/components/layout/Header'
import PrizeDrawForm from '@/components/PrizeDrawForm'

import Container from '@/components/layout/Container'
import Spinner from '@/components/UI/Spinner'

import ErrorAlert from '@/components/UI/ErrorAlert'

import Button from '@/components/UI/Button'

import dataAccess from '@/axios-base';

import FormState from '@/models/FormState';

export default {
  name: 'Home',
  components: {
    Header,
    PrizeDrawForm,
    Container, 
    Spinner,
    ErrorAlert,
    Button
  },
  data() {
    return {
        submissions: 0,
        formState: null,
        config: null,
        validationRules: {
          firstNameRules: null,
          lastNameRules: null,
          emailRules:  null,
          serialNumberRules: null
        },
        error: null,
        state: ''
    }
  },
  computed: {
      displayForm(){
        return this.formState === FormState.READY;
      },
      displayError() {
        return this.error != null
      },
      displaySpinner() {
        return this.formState === FormState.SENDING | FormState.INITIAL;
      },
      displayConfirmation(){
        return this.formState === FormState.SUBMITTED;
      }
  },
  methods: {

      successfulPrizeDrawSubmit(entry) {

        this.setFormState(FormState.SENDING);
        this.error = null;

        dataAccess.post('/PrizeDraw/SubmitEntry', entry)
          .then(() => {

            this.submissions = this.submissions += 1;
            this.setFormState(FormState.SUBMITTED);

          })
          .catch(err => { 
            
            if(err.response.status === 400)
            {
              const error = { 
                summary: err.response.data.value.message,
                errors: err.response.data.value.errors
              }
              
              this.error = error;
            }

            this.setFormState(FormState.READY);
          });
      },

      tryAgain()
      {
        this.setFormState(FormState.READY);
      },
      setFormState(formState){

        this.formState = formState;
      }
  },
  beforeCreate(){
      
      dataAccess.get('/Config')
        .then(response => {

          this.config = response.data;
          this.validationRules = response.data.validation;

          this.setFormState(FormState.READY);
        })
        .catch(() => {
          
          const error = { 
                summary: "Error connecting to the API. Please refresh the page to try again",
                errors: null
              }
              
          this.error = error;

          this.setFormState(FormState.HIDDEN);
        });
  }
}

</script>
