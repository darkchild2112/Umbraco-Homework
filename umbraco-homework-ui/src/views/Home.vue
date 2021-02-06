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
          v-on:successfulSubmit="successfulPrizeDrawSubmit"/>

        <Spinner v-if="displaySpinner"/>

        <div v-if="displayConfirmation" class="text-center">
          <h2>Entry Successful</h2>
          <p>Thank you for submitting the form</p>
          <template v-if="submissions < config.maxSubmissions">
            <p>You can enter the prize draw a maximum number of {{ config.maxSubmissions }} times. Would you like to try again?</p>
            <Button v-on:click="tryAgain" :text="'Submit Entry'" />
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
        formState: FormState.INITIAL,
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
        return this.formState === FormState.INITIAL;
      },
      displayError() {
        return this.error != null
      },
      displaySpinner() {
        return this.formState === FormState.SENDING;
      },
      displayConfirmation(){
        return this.formState === FormState.SUBMITTED;
      }
  },
  methods: {

      successfulPrizeDrawSubmit(entry) {

        this.formState = FormState.SENDING;
        this.error = null;

        dataAccess.post('/PrizeDraw/SubmitEntry', entry)
          .then(response => {

            console.log('Success:', response);

            this.submissions = this.submissions += 1;
            this.formState = FormState.SUBMITTED;

            console.log('prize draw successfully submitted');
            console.log(`successfully submitted ${this.submissions} times`);
            console.log(entry);
          })
          .catch(err => { 
            console.log(err.response);
            
            if(err.response.status === 400)
            {
              const error = { 
                summary: err.response.data.value.message,
                errors: err.response.data.value.errors
              }
              
              this.error = error;
            }

            this.formState = FormState.INITIAL;
          });
      },

      tryAgain()
      {
        this.formState = FormState.INITIAL;
      }
  },
  beforeCreate(){
      
      dataAccess.get('/Config')
        .then(response => {

          console.log(response);

          this.config = response.data;
          this.validationRules = response.data.validation;

          console.log(FormState.INITIAL);


        })
        .catch(err => {
          
          const error = { 
                summary: "Error Contacting the API - Please refresh the page to try again",
                errors: null
              }
              
              this.error = error;

          console.log(err)}
          
        );
  }
}

</script>
