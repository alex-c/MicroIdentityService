<template>
  <div id="identities" class="standard-page">
    <IdentityList />

    <!-- TODO: password reset links -->
  </div>
</template>

<script>
import IdentityList from '@/views/private/identities/IdentityList.vue';

export default {
  name: 'Identities',
  components: { IdentityList },
};
</script>
