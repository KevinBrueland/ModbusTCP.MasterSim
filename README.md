# ModbusTCP.MasterSim
ModbusTCP.MasterSim is a Modbus TCP master simulator built on the NModbus library. It supports all the standard read/write functionality.

# AppSettings.json
The appsettings.json config file has three application settings that needs to be set:

## SlaveIPAddress

The IP-address of the slave you wish to connect to. 

**Example:** 127.0.0.1

---
---

## SlaveTcpPort
The TCP port of the slave you wish to connect to. 

**Example:** 502

---
---

## SlaveId

The slaveId you wish to send commands to.

**Example:** 1

---
---


# MODBUS Commands


## Read range of coils
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
### Write to a range of coils
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
### Read a range of holding registers as bits
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

## Set/change slave ID
**Command:** 

SET SLAVEID

**Arguments:** 

--value (byte)

**Description**
Sets the slave ID the client sends commands to.


**Example**

SET SLAVEID --value 1

#
#

## Set/change slave IP-address
**Command:** 

SET IPADDRESS

**Arguments:** 

--value (ip-address in format x.x.x.x)

**Description**
Sets the IP-address of the slave the client connects to.
This will force any current connection to be closed and will then attempt to reconnect to the new IP-address with the existing TCP port.


**Example**

SET IPADDRESS --value 127.0.0.1


#
#


## Set/change slave TCP port
**Command:** 

SET PORT

**Arguments:** 

--value (int)

**Description**
Sets the TCP port of the slave the client connects to.
This will force any current connection to be closed and will then attempt to reconnect to the new TCP port with the existing IP-address.


**Example**

SET PORT --value 502



