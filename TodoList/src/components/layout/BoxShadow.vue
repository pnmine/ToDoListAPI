<template>
  <div class="box-shadow">
    <slot></slot>
  </div>
</template>
<style scoped>
.box-shadow {
  border-radius: 12px;
  box-shadow: rgba(17, 12, 46, 0.15) 0px 48px 100px 0px;
  padding: 1em;
  margin: 1em auto;
}
</style>
