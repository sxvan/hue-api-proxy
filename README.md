# hue-api-proxy

## Usage

### Toggle lights

`<ip>/api/v1/toggle/<id>`

### Toggle

`<ip>/api/v1/scene/<id>`

## Overview

`<ip>/api/v1/overview/lights`

`<ip>/api/v1/overview/scenes`

## myStrom Button integration

Buttons

- `btn1`, `btn1`, `btn3`, `btn4`

Actions

- `single`, `double`, `long`

Set button

```shell
curl --location -g --request POST 'http://<button_ip>/api/v1/action/<button>/<action>' \
--data-raw get://<server_ip>/api/v1/toggle/<id>
```

Get Button

```shell
curl --location -g --request GET '<button_ip>/api/v1/actions/<button>'
```