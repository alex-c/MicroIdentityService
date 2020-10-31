<template>
  <svg xmlns="http://www.w3.org/2000/svg" :width="size" :height="size" viewBox="0 0 24 24">
    <path :d="path" />
  </svg>
</template>

<script>
export default {
  name: 'Icon',
  props: {
    size: {
      type: Number,
      default: 24,
    },
    path: {
      type: String,
      required: true,
    },
  },
};
</script>

<style scoped>
path {
  fill: currentColor;
}
</style>
