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
          firstName: [{ regex: '\\S', message: 'first name is mandatory' }],
          lastName: [{ regex: '\\S', message: 'last name is mandatory' }],
          email: [{ regex: '\\S', message: 'email is mandatory' }, { regex: "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:.[a-zA-Z0-9-]+)*$", message: 'email needs to be in the format xxx@sss.xxx' }],
          serialNumber: [{ regex: '\\S', message: 'serial number is mandatory' }]
        }
    }
  },
  methods: {

      updateInput(event) {

        console.log(event);
        this.formState = event;
      },
      successfulPrizeDrawSubmit(entry) {

        this.submissions = this.submissions += 1;
        this.formState = 'submitted';

        console.log('prize draw successfully submitted');
        console.log(`successfully submitted ${this.submissions} times`);
        console.log(entry);
      },

      tryAgain()
      {
        this.formState = 'initial';
      }
  },
  mounted(){
      
      fetch('https://localhost:5001/Config')
        .then(response => response.json())
        .then(data => this.config = data)
        .catch(err => console.log(err));
  }
}

</script>
