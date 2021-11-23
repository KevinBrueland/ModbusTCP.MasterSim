## ModbusTCP.MasterSim
ModbusTCP.MasterSim is a Modbus TCP master simulator built on the NModbus library. It supports all the standard read/write functionality.

- [Getting started](#getting-started)
- [AppSettings.json <a name="appsettings"></a>](#appsettingsjson-)
  - [SlaveIPAddress <a name="slaveipaddress"></a>](#slaveipaddress-)
  - [SlaveTcpPort <a name="slavetcpport"></a>](#slavetcpport-)
  - [SlaveId <a name="slaveid"></a>](#slaveid-)
  - [MaxRetryCount](#maxretrycount)
  - [RetryInterval](#retryinterval)
  - [SendTimeout](#sendtimeout)
  - [ReceiveTimeout](#receivetimeout)
- [MODBUS Commands](#modbus-commands)
  - [Read a range of coils](#read-a-range-of-coils)
  - [Write to a single coil](#write-to-a-single-coil)
  - [Write to a range of coils](#write-to-a-range-of-coils)
  - [Read a range of discrete inputs](#read-a-range-of-discrete-inputs)
  - [Read a range of input registers](#read-a-range-of-input-registers)
  - [Read a range of input registers as bits](#read-a-range-of-input-registers-as-bits)
  - [Read a range of input registers as 32-bit floats](#read-a-range-of-input-registers-as-32-bit-floats)
  - [Read a range of holding registers](#read-a-range-of-holding-registers)
  - [Read a range of holding registers as bits](#read-a-range-of-holding-registers-as-bits)
  - [Read a range of holding registers as 32-bit floats](#read-a-range-of-holding-registers-as-32-bit-floats)
  - [Write to a range of holding registers](#write-to-a-range-of-holding-registers)
- [Client Commands](#client-commands)
  - [Connect to slave](#connect-to-slave)
  - [Reconnect to slave](#reconnect-to-slave)
  - [Set/change slave ID](#setchange-slave-id)
  - [Set/change slave IP-address](#setchange-slave-ip-address)
  - [Set/change slave TCP port](#setchange-slave-tcp-port)


#
#


# Getting started

To get started with the ModbusTCP master simulator, clone the repository and build the project.


# AppSettings.json <a name="appsettings"></a>
The appsettings.json config file has application settings that can be set in order to autoconnect to a slave.
If these values arent present in the appsettings.json file, you can still manually connect to a slave with the 'Connect' command.

## SlaveIPAddress <a name="slaveipaddress"></a>

The IP-address of the slave you wish to connect to. 

**Example:** 127.0.0.1

---
---

## SlaveTcpPort <a name="slavetcpport"></a>
The TCP port of the slave you wish to connect to. 

**Example:** 502

---
---

## SlaveId <a name="slaveid"></a>

The slaveId you wish to send commands to.

**Example:** 1

---
---

## MaxRetryCount 

Max number of retries when attempting to establish a connection to the slave. Value in ms.

**Example:** 5000

---
---

## RetryInterval 

Time between each retry attempt. Value in ms.

**Example:** 5000

---
---

## SendTimeout 

Send time before timing out. Value in ms.

**Example:** 5000

---
---

## ReceiveTimeout 

Receive time before timing out. Value in ms.

**Example:** 5000

---
---


# MODBUS Commands


## Read a range of coils
**Command:** 

READ COILS

**Arguments:** 

--startaddress (uint16)

--numbertoread (uint16)

**Description**

Reads **--numbertoread** coil values starting from **--startaddress**

**Example**

READ COILS --startaddress 1 --numbertoread 10

#
#

## Write to a single coil
**Command:** 

WRITE COIL

**Arguments:** 

--address (uint16)

--value (bool)

**Description**

Writes the **--value** to the **--address**

**Example**

WRITE COIL --address 1 --value true

#
#


## Write to a range of coils
**Command:** 

WRITE COILS

**Arguments:** 

--startaddress (uint16)

--values (comma separated string of bool)

**Description**

Writes the **--values** to coil addresses starting at **--startingaddress** and increments address by one for each value.

**Example**

WRITE COILS --startaddress 1 --value true,true,false,true

#
#


## Read a range of discrete inputs
**Command:** 

READ DISCRETES

**Arguments:** 

--startaddress (uint16)

--numbertoread (uint16)

**Description**

Reads **--numbertoread** discrete input values starting from **--startaddress**

**Example**

READ DISCRETES --startaddress 10001 --numbertoread 10

#
#

## Read a range of input registers
**Command:** 

READ INPUTREGS

**Arguments:** 

--startaddress (uint16)

--numbertoread (uint16)

**Description**

Reads **--numbertoread** input register values starting from **--startaddress**

**Example**

READ INPUTREGS --startaddress 30001 --numbertoread 5


#
#


## Read a range of input registers as bits
**Command:** 

READ INPUTREGSB

**Arguments:** 

--startaddress (uint16)

--numbertoread (uint16)

**Description**

Reads **--numbertoread** input register values starting from **--startaddress** and displays the individual bit values.

**Example**

READ INPUTREGSB --startaddress 30001 --numbertoread 5


#
#


## Read a range of input registers as 32-bit floats
**Command:** 

READ INPUTREGSF

**Arguments:** 

--startaddress (uint16)

--numbertoread (uint16)

**Description**

Reads **--numbertoread** input register pairs starting from **--startaddress** and displays the value as 32-bit floats.

NOTE: Register values in Modbus are 16 bit unsigned values, so a 32-bit float consists of two consecutive Uint16 register address values converted to a 32-bit float.

**Example**

READ INPUTREGSF --startaddress 30001 --numbertoread 3


#
#


## Read a range of holding registers
**Command:** 

READ HOLDREGS

**Arguments:** 

--startaddress (uint16)

--numbertoread (uint16)

**Description**

Reads **--numbertoread** holding register values starting from **--startaddress**.

**Example**

READ HOLDREGS --startaddress 40001 --numbertoread 5

#
#


## Read a range of holding registers as bits
**Command:** 

READ HOLDREGSB

**Arguments:** 

--startaddress (uint16)

--numbertoread (uint16)

**Description**

Reads **--numbertoread** holding register values starting from **--startaddress** and displays the individual bit values.

**Example**

READ HOLDREGSB --startaddress 40001 --numbertoread 5


#
#


## Read a range of holding registers as 32-bit floats
**Command:** 

READ HOLDREGSF

**Arguments:** 

--startaddress (uint16)

--numbertoread (uint16)

**Description**

Reads **--numbertoread** holding register pairs starting from **--startaddress** and displays the values as 32-bit floats.

NOTE: Register values in Modbus are 16 bit unsigned values, so a 32-bit float consists of two consecutive Uint16 register address values converted to a 32-bit float.

**Example**

READ HOLDREGSF --startaddress 40001 --numbertoread 3


#
#


## Write to a range of holding registers
**Command:** 

WRITE HOLDREGS

**Arguments:** 

--startaddress (uint16)

--values (see description)

**Description**

Writes the **--values** to holding register addresses starting at **--startingaddress** and increments address by one for each value.

The **--values** argument expects a comma-separated list of one of the following:

1. 16-bit unsigned values
2. 16-bit bitstrings (LSB->MSB)
3. 32-bit floats

Note that you cannot mix types for a single write. The comma-separated values must consist of the same type.


**Example**

WRITE HOLDREGS --startaddress 40001 --values 1234,20543,51534

WRITE HOLDREGS --startaddress 40001 --values 1001000000001100,0001011000110000

WRITE HOLDREGS --startaddress 40001 --values 32.1235,654.2534,99.33452

#
#

# Client Commands

## Connect to slave
**Command:** 

CONNECT

**Arguments:** 

--ipaddress (ip-address in format x.x.x.x)

--port (int)

--slaveid (byte)

**Description**

Connects to a slave with the provided ip, port and slaveid.


**Example**

CONNECT --ipaddress 127.0.0.1 --port 502 --slaveid 1

#
#

## Reconnect to slave
**Command:** 

RECONNECT

**Arguments:** 

N/A

**Description**

Reconnects to the last connected slave


**Example**

RECONNECT

#
#

## Set/change slave ID
**Command:** 

SET SLAVEID

**Arguments:** 

--slaveid (byte)

**Description**

Sets the slave ID the client sends commands to.


**Example**

SET SLAVEID --slaveid 1

#
#

## Set/change slave IP-address
**Command:** 

SET IPADDRESS

**Arguments:** 

--ipaddress (ip-address in format x.x.x.x)

**Description**

Sets the IP-address of the slave the client connects to.
This will force any current connection to be closed and will then attempt to reconnect to the new IP-address with the existing TCP port.


**Example**

SET IPADDRESS --ipaddress 127.0.0.1


#
#


## Set/change slave TCP port
**Command:** 

SET PORT

**Arguments:** 

--port (int)

**Description**

Sets the TCP port of the slave the client connects to.
This will force any current connection to be closed and will then attempt to reconnect to the new TCP port with the existing IP-address.


**Example**

SET PORT --port 502



