# Macro Deck GPU-Z Plugin

![GitHub](https://img.shields.io/github/license/RecklessBoon/Macro-Deck-Discord-Plugin)

A plugin for Macro Deck 2 to utilize the GPU-Z shared memory data to get live hardware statistics as variables.

If you like my work and want to support/encourage me in making more plugins, you certainly can do so on Ko-Fi.

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Z8Z37FRBM)

## Features

### Variables
At the time of this writing, this plugin updates the following variables for use anywhere variables are
allowed in Macro Deck 2. I'll leave it to you to figure out what each can be used for.

| Variable Name              | 
| -------------------------- | 
| gpu_z_hasboostclocks |
| gpu_z_membandwidth |
| gpu_z_biosuefi |
| gpu_z_biosversion |
| gpu_z_bus |
| gpu_z_businterface |
| gpu_z_cuda |
| gpu_z_cuda_capability |
| gpu_z_cardname |
| gpu_z_clockgpu |
| gpu_z_clockgpubase |
| gpu_z_clockgpubasedefault |
| gpu_z_clockgpuboost |
| gpu_z_clockgpuboostdefault |
| gpu_z_clockgpudefault |
| gpu_z_clockmem |
| gpu_z_clockmemdefault |
| gpu_z_clockshader |
| gpu_z_clockshaderdefault |
| gpu_z_dxcompute |
| gpu_z_dxcompute_capability |
| gpu_z_dxr |
| gpu_z_dev |
| gpu_z_deviceid |
| gpu_z_diesize |
| gpu_z_directml |
| gpu_z_directxsupport |
| gpu_z_driverdate |
| gpu_z_driverversion |
| gpu_z_fillratepixel |
| gpu_z_fillratetexel |
| gpu_z_fn |
| gpu_z_gpuname |
| gpu_z_gpurevision |
| gpu_z_membuswidth |
| gpu_z_memsize |
| gpu_z_memsizefordisplay |
| gpu_z_memtype |
| gpu_z_memvendor |
| gpu_z_monitor_1\aspectratio |
| gpu_z_monitor_1\dpmssupport |
| gpu_z_monitor_1\displaydiagonal |
| gpu_z_monitor_1\displayheight |
| gpu_z_monitor_1\displaywidth |
| gpu_z_monitor_1\gamma |
| gpu_z_monitor_1\hrefreshmax |
| gpu_z_monitor_1\hrefreshmin |
| gpu_z_monitor_1\manufactured |
| gpu_z_monitor_1\maxresx |
| gpu_z_monitor_1\maxresy |
| gpu_z_monitor_1\model |
| gpu_z_monitor_1\serial |
| gpu_z_monitor_1\type |
| gpu_z_monitor_1\vrefreshmax |
| gpu_z_monitor_1\vrefreshmin |
| gpu_z_monitorcount |
| gpu_z_multigpu0 |
| gpu_z_multigpuname |
| gpu_z_numrops |
| gpu_z_numshaderspixel |
| gpu_z_numshadersunified |
| gpu_z_numshadersvertex |
| gpu_z_numtmus |
| gpu_z_opencl |
| gpu_z_opencl_profile |
| gpu_z_opencl_version |
| gpu_z_opengl |
| gpu_z_opengl_version |
| gpu_z_physx |
| gpu_z_processsize |
| gpu_z_releasedate |
| gpu_z_resizablebar |
| gpu_z_subsysid |
| gpu_z_subvendor |
| gpu_z_subvendorid |
| gpu_z_transistors |
| gpu_z_vendor |
| gpu_z_vendorid |
| gpu_z_vulkan |
| gpu_z_vulkan_version |
| gpu_z_whql |
| gpu_z_gpu_clock_unit |
| gpu_z_gpu_clock_digits |
| gpu_z_gpu_clock_value |
| gpu_z_memory_clock_unit |
| gpu_z_memory_clock_digits |
| gpu_z_memory_clock_value |
| gpu_z_gpu_temperature_unit |
| gpu_z_gpu_temperature_digits |
| gpu_z_gpu_temperature_value |
| gpu_z_gpu_power_unit |
| gpu_z_gpu_power_digits |
| gpu_z_gpu_power_value |
| gpu_z_gpu_voltage_unit |
| gpu_z_gpu_voltage_digits |
| gpu_z_gpu_voltage_value |
| gpu_z_gpu_load_unit |
| gpu_z_gpu_load_digits |
| gpu_z_gpu_load_value |
| gpu_z_memory_used_(dedicated)_unit |
| gpu_z_memory_used_(dedicated)_digits |
| gpu_z_memory_used_(dedicated)_value |
| gpu_z_cpu_temperature_unit |
| gpu_z_cpu_temperature_digits |
| gpu_z_cpu_temperature_value |
| gpu_z_system_memory_used_unit |
| gpu_z_system_memory_used_digits |
| gpu_z_system_memory_used_value |


### Actions
This plugin adds the following actions

| Action            | Description                               |
| ----------------- | :---------------------------------------- |
| Refresh Variables | Manually trigger polling and variable updates immediately |

## Installation
Download/Install it directly in Macro Deck from the package manager.

## Configuration

### Prerequisites

<p>In order for this plugin to work at all, the program GPU-Z must be downloaded <b>and 
running</b>.</p>

You can find more information on what it is and how to get it running here:<br/>
<b>[GPU-Z Graphics Card GPU Information Utility](https://www.techpowerup.com/gpuz/)</b>

### Macro Deck 2
1. Open Macro Deck 2 application on your computer
2. Switch to the plugin manager view and locate the GPU-Z plugin
3. Click the "Configure" button
4. Set the polling frequency to whatever you like above 5 seconds

After that, you should be able to watch the `gpu_z_*` variables change after ever
poll

## Addendum

### Not a Standalone App
<img align="right" height="64px" src="https://macrodeck.org/images/works_with_macrodeck2.png" />

This is a plugin for [Macro Deck 2](https://github.com/SuchByte/Macro-Deck), it does **NOT** function as a standalone app

### Third Party Licenses / Thank you

This plugin utilizes other projects/solutions. Here are their licenses:

- [Newtonsoft.Json](https://www.newtonsoft.com/json)
- [GPU-Z](https://www.techpowerup.com/gpuz/)
