<template>
  <div>
    <Header />

    <PrizeDrawForm 
      v-if="this.formState === 'initial'" 
      v-bind:validationRules="this.validationRules"
      v-on:successfulSubmit="successfulPrizeDrawSubmit"/>

    <div v-if="this.formState === 'submitted'">
      <p>Thank you for submitting the form</p>
      <template v-if="submissions < config.maxSubmissions">
        <button v-on:click="tryAgain">Try again</button>
      </template>
    </div>
  </div>
</template>

<script>

import Header from '@/components/layout/Header'
import PrizeDrawForm from '@/components/PrizeDrawForm'

export default {
  name: 'Home',
  components: {
    Header,
    PrizeDrawForm
  },
  data() {
    return {
        submissions: 0,
        formState: 'initial',
        config: null,
        validationRules: {
          firstNameRules: null,
          lastNameRules: null,
          emailRules:  null,
          serialNumberRules: null
        }
    }
  },
  methods: {

      updateInput(event) {

        console.log(event);
        this.formState = event;
      },
      successfulPrizeDrawSubmit(entry) {

        const options = {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(entry),
        };

        fetch('https://localhost:5001/PrizeDraw/SubmitEntry', options)
        .then(data => {
          console.log('Success:', data);

          this.submissions = this.submissions += 1;
          this.formState = 'submitted';

          console.log('prize draw successfully submitted');
          console.log(`successfully submitted ${this.submissions} times`);
          console.log(entry);
        })
        .catch(error => console.log('Error:', error.json()));
      },

      tryAgain()
      {
        this.formState = 'initial';
      }
  },
  mounted(){
      
      fetch('https://localhost:5001/Config')
        .then(response => response.json())
        .then(data => {

          console.log(data);

          this.config = data;
          this.validationRules = data.validation;
        })
        .catch(err => console.log(err));
  }
}

</script>
